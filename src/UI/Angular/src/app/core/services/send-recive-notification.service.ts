import { Injectable, inject } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { AlertService } from '../../shared/services/alert.service';
import { AuthenticationService } from './authentication.service';
import { APIs } from '../../shared/constants/api-endpoints';
import { EnvironmentService } from './environment.service';

@Injectable({
  providedIn: 'root',
})
export class SendReciveNotificationService {
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

      this.connection.on('ReceiveTaskCreation', (obj: any) => {
        console.log(obj);
        this.alertService.showMessage(
          'Task: ' + obj.taskName + ' has been assigned to you.',
        );
      });
    }, 2000);
  }

  public async leaveSignalRConnection() {
    console.log('Connection is lost!');
    return this.connection.stop();
  }
}
