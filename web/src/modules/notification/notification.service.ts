import { Injectable } from '@angular/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { NotificationType } from './notification-type';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  constructor(private readonly notification: NzNotificationService) {}

  notify(notificationType: NotificationType, title: string, message: string) {
    if (notificationType === NotificationType.Success) {
      this.notification.success(title, message, { nzDuration: 30000, nzPlacement: 'bottomRight' });
    } else if (notificationType === NotificationType.Error) {
      this.notification.error(title, message, { nzDuration: 30000, nzPlacement: 'bottomRight' });
    }
  }
}
