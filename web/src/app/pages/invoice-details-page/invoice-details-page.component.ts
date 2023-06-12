import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { NotificationService } from '../../../modules/notification/notification.service';
import { errorHandler, handleErrors } from '../../../modules/operators/handleErrors';
import { NotificationType } from '../../../modules/notification/notification-type';
import { PageState } from '../../../modules/page/page-state';
import { InvoiceService } from '../../../modules/invoice/invoice.service';
import { Invoice } from '../../../modules/invoice/invoice.model';
import { Button } from '../../../modules/common-components/button/button.model';
import { AppRoutes } from '../../routing/app-routes';
import { DeleteConfirmationService } from '../../../modules/forms/delete-confirmation/delete-confirmation.service';
import { PermissionsService } from '../../../modules/permissions/permissions.service';
import { BipValidators } from '../../../modules/forms/bip-validators';
import { BipFormGroup } from '../../../modules/forms/form-controls/bip-form-group';
import { FormState } from '../../../modules/forms/form-modal/form-modal-content/form-state';
import { BipFormControl } from '../../../modules/forms/form-controls/bip-form-control';
import { FormModalService } from '../../../modules/forms/form-modal/form-modal.service';
import { UpdateInvoiceModalContentComponent } from './update-invoice-modal-content/update-invoice-modal-content.component';
import { UpdateInvoiceModalContentState } from './update-invoice-modal-content/update-invoice-modal-content-state';

@UntilDestroy()
@Component({
  selector: 'bip-purchase-order-details-page',
  templateUrl: './invoice-details-page.component.html',
  styleUrls: ['./invoice-details-page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InvoiceDetailsPageComponent implements OnInit {
  headerButtons: Button[] = [];

  pageState: PageState = PageState.loaded;

  invoice?: Invoice;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly invoiceService: InvoiceService,
    private readonly notificationService: NotificationService,
    private readonly deleteConfirmationService: DeleteConfirmationService,
    private readonly formModalService: FormModalService,
    private readonly router: Router,
    private readonly permissionsService: PermissionsService,
    private readonly changeDetectorRef: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    if (this.permissionsService.containsPermissions({ invoice: { canWrite: true } })) {
      this.headerButtons = [
        ...this.headerButtons,
        {
          type: 'default',
          text: 'Edit',

          onClick: () => this.onUpdateInvoice(),
        },
        {
          type: 'danger',
          text: 'Delete',

          onClick: () => this.onDeleteInvoice(),
        },
      ];
    }

    this.fetchInvoice();
  }

  fetchInvoice() {
    this.pageState = PageState.loading;

    this.route.params
      .pipe(
        untilDestroyed(this),
        switchMap((params) => this.invoiceService.getInvoice(params['invoiceNumber'])),
        handleErrors([
          errorHandler(() => {
            this.notificationService.notify(NotificationType.Error, 'Error', 'Unknown error has occurred while loading invoice');
            this.pageState = PageState.error;
          }),
        ]),
      )
      .subscribe((invoice) => {
        this.invoice = invoice;
        this.pageState = PageState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }

  private onUpdateInvoice() {
    const invoice = this.invoice!;
    const formGroup: BipFormGroup = new BipFormGroup({
      number: new BipFormControl<string>(invoice.number, 'Number', { validators: [BipValidators.required()] }),
      amount: new BipFormControl<number>(invoice.amount, 'Amount', { validators: [BipValidators.required()] }),
      receivedDate: new BipFormControl<Date>(invoice.receivedDate, 'Received date', { validators: [BipValidators.required()] }),
      dueDate: new BipFormControl<Date>(invoice.dueDate, 'Due date', { validators: [BipValidators.required()] }),
    });

    const onSubmit = () => {
      this.notificationService.notify(NotificationType.Success, 'Success', `The invoice ${invoice.number} was updated`);
      this.router.navigate([AppRoutes.Invoices, formGroup.bipControls.number.value]);
    };
    const submitEnabled = (formState: FormState) => {
      if (formState == FormState.Loading || formState == FormState.Submitting) {
        return false;
      }

      return (
        invoice.number != formGroup.bipControls.number.value ||
        invoice.amount != formGroup.bipControls.amount.value ||
        invoice.receivedDate != formGroup.bipControls.receivedDate.value ||
        invoice.dueDate != formGroup.bipControls.dueDate.value
      );
    };

    this.formModalService.show<UpdateInvoiceModalContentComponent, UpdateInvoiceModalContentState>(
      'Update invoice',
      UpdateInvoiceModalContentComponent,
      formGroup,
      onSubmit,
      submitEnabled,
      { invoice },
    );
  }

  private onDeleteInvoice() {
    const invoiceNumber = this.invoice!.number;
    const onDelete = (): void => {
      this.router.navigate([AppRoutes.Invoices]);
      this.notificationService.notify(NotificationType.Success, 'Success', `Invoice ${invoiceNumber} was deleted`);
    };

    this.deleteConfirmationService.confirmDelete(
      'Are you sure to delete invoice?',
      invoiceNumber,
      this.invoiceService.deleteInvoice(invoiceNumber),
      onDelete,
    );
  }
}
