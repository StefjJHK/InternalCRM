import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { BipFormGroup } from '../../../modules/forms/form-controls/bip-form-group';
import { BipFormControl } from '../../../modules/forms/form-controls/bip-form-control';
import { BipValidators } from '../../../modules/forms/bip-validators';
import { Button } from '../../../modules/common-components/button/button.model';
import { AuthAccessService } from '../../../modules/auth/auth-access.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { GlobalLoaderService } from '../../../modules/loader/global-loader/global-loader.service';
import { errorHandler, errorHandlerByType, handleErrors } from '../../../modules/operators/handleErrors';
import { LoginError } from '../../../modules/auth/login-error';
import { NotificationService } from '../../../modules/notification/notification.service';
import { NotificationType } from '../../../modules/notification/notification-type';
import { Router } from '@angular/router';
import { AppRoutes } from '../../routing/app-routes';

@UntilDestroy()
@Component({
  selector: 'bip-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginPageComponent implements OnInit {
  errorMessage: string | null = null;

  formGroup = new BipFormGroup({
    login: new BipFormControl<string>('', 'Username', { validators: [BipValidators.required()] }),
    password: new BipFormControl<string>('', 'Password', { validators: [BipValidators.required()] }),
  });

  loginButton: Button = {
    text: 'Log in',
    type: 'primary',
    onClick: () => this.login(),
  };

  constructor(
    private readonly authService: AuthAccessService,
    private readonly globalLoaderService: GlobalLoaderService,
    private readonly notificationService: NotificationService,
    private readonly router: Router,
  ) {}

  ngOnInit() {
    this.formGroup.valueChanges.subscribe(() => {
      if (this.errorMessage) {
        this.errorMessage = null;
      }
    });
  }

  login() {
    this.errorMessage = null;
    this.formGroup.markAllAsTouched();

    if (this.formGroup.invalid) {
      return;
    }

    this.authService
      .login({ password: this.formGroup.bipControls.password.value, username: this.formGroup.bipControls.login.value })
      .pipe(
        untilDestroyed(this),
        this.globalLoaderService.wrapPipe(),
        handleErrors([
          errorHandlerByType((err) => {
            this.errorMessage = err.message;
            this.formGroup.markAllAsTouched();
          }, LoginError),
          errorHandler(() => {
            this.notificationService.notify(NotificationType.Error, 'Login failed', 'An unknown error has occurred');
          }),
        ]),
      )
      .subscribe(() => {
        this.router.navigate([AppRoutes.Dashboard]);
      });
  }
}
