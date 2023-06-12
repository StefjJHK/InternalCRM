import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../operators/handleErrors';
import { WidgetState } from '../../widget/widget-state';
import { Lead } from '../lead.model';
import { LeadService } from '../lead.service';

@UntilDestroy()
@Component({
  selector: 'bip-leads-table-widget',
  templateUrl: './leads-table-widget.component.html',
  styleUrls: ['./leads-table-widget.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LeadsTableWidgetComponent implements OnInit {
  @Input() productName?: string;

  widgetState: WidgetState = WidgetState.loaded;

  leads: Lead[] = [];

  constructor(private readonly leadService: LeadService, private readonly changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit() {
    this.fetchLeads();
  }

  fetchLeads() {
    this.widgetState = WidgetState.loading;

    this.leadService
      .getLeads({ product: this.productName })
      .pipe(
        untilDestroyed(this),
        handleErrors([
          errorHandler(() => {
            this.widgetState = WidgetState.error;

            this.changeDetectorRef.markForCheck();
          }),
        ]),
      )
      .subscribe((leads: Lead[]) => {
        this.leads = leads;
        this.widgetState = WidgetState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }
}
