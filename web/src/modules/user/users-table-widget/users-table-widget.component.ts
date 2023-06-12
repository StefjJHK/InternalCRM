import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { WidgetState } from '../../widget/widget-state';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../operators/handleErrors';
import { User } from '../user.model';
import { UserService } from '../user.service';
import { Button } from '../../common-components/button/button.model';
import { DeleteConfirmationService } from '../../forms/delete-confirmation/delete-confirmation.service';
import { NotificationService } from '../../notification/notification.service';
import { NotificationType } from '../../notification/notification-type';

@UntilDestroy()
@Component({
  selector: 'bip-users-table-widget',
  templateUrl: './users-table-widget.component.html',
  styleUrls: ['./users-table-widget.component.less'],
})
export class UsersTableWidgetComponent implements OnInit {
  widgetState: WidgetState = WidgetState.loaded;

  users?: User[];

  constructor(
    private readonly userService: UserService,
    private readonly deleteConfirmationService: DeleteConfirmationService,
    private readonly notificationService: NotificationService,
    private readonly changeDetectorRef: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.fetchUsers();
  }

  fetchUsers() {
    this.widgetState = WidgetState.loading;

    this.userService
      .getUsers()
      .pipe(
        untilDestroyed(this),
        handleErrors([
          errorHandler(() => {
            this.widgetState = WidgetState.error;

            this.changeDetectorRef.markForCheck();
          }),
        ]),
      )
      .subscribe((users) => {
        this.users = users;
        this.widgetState = WidgetState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }

  deleteUser(user: User) {
    const onDelete = (): void => {
      this.notificationService.notify(NotificationType.Success, 'Success', `User ${user.username} was deleted`);

      this.fetchUsers();
    };

    this.deleteConfirmationService.confirmDelete(
      'Are you sure to delete user?',
      user.username,
      this.userService.deleteUser(user.username),
      onDelete,
    );
  }

  createDeleteAction(user: User): Button {
    return {
      text: 'delete',
      type: 'text',

      onClick: () => this.deleteUser(user),
    };
  }
}
