import { Injectable, inject, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { AlertService } from '../../shared/services/alert.service';
import { AuthenticationService } from './authentication.service';
import { APIs } from '../../shared/constants/api-endpoints';
import { EnvironmentService } from './environment.service';
import { INotificationMessage } from '../models/notification-message.interface';
import { CommonService } from './common.service';
import { appPaths } from '../../shared/constants/routes';

@Injectable({
  providedIn: 'root',
})
export class SendReciveNotificationService {
  notifications = signal<INotificationMessage[]>([]);

  commonService = inject(CommonService);
  alertService = inject(AlertService);
  authenticationService = inject(AuthenticationService);
  environmentService = inject(EnvironmentService);
  private connection!: signalR.HubConnection;

  constructor() {
    setTimeout(() => {
      var token = `${this.authenticationService.getToken()}`;
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(
          `${this.environmentService.domainUrl}${APIs.getNotifications}`,
          {
            accessTokenFactory: () => token,
          },
        )
        .configureLogging(signalR.LogLevel.Error)
        .build();

      this.connection
        .start()
        .then(() => console.log('Connection started'))
        .catch((err) => console.log('Error while starting connection: ' + err));

      this.connection.on(
        'ReceiveMessage',
        (message: string, messageTime: string) => {
          console.log(message);
        },
      );

      this.connection.on(
        'ReceiveTaskCreation',
        (obj: any, messageTime: any) => {
          let taskObj = {
            id: obj.taskId,
            message: `<strong class="text-sm mr-1">New task </strong>${obj.taskName}<strong> assigned.</strong>`,
            time: this.commonService.formatCustomDateTime(messageTime),
            routePath: '/' + appPaths.tasksDetail + '/' + obj.taskId,
          } as INotificationMessage;
          this.addNotification(taskObj);
        },
      );
    }, 4000);
  }

  public async leaveSignalRConnection() {
    console.log('Connection is lost!');
    return this.connection.stop();
  }

  addNotification(notification: INotificationMessage): void {
    this.notifications.update((items) => [...items, notification]);
  }

  removeNotification(id: string): void {
    this.notifications.update((items) =>
      items.filter((item) => item.id !== id),
    );
  }
}
