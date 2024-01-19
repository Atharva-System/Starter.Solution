import { Injectable, inject, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { AlertService } from '../../shared/services/alert.service';
import { AuthenticationService } from './authentication.service';
import { APIs } from '../../shared/constants/api-endpoints';
import { EnvironmentService } from './environment.service';
import { INotificationMessage } from '../models/notification-message.interface';
import { CommonService } from './common.service';
import { appPaths } from '../../shared/constants/routes';
import {
  IChatMessage,
  IChatUser,
  IUserTyping,
} from '../models/chat-user.interface';

@Injectable({
  providedIn: 'root',
})
export class SendReciveNotificationService {
  notifications = signal<INotificationMessage[]>([]);
  chatUsers = signal<IChatUser[]>([]);
  isUserTyping = signal<IUserTyping>({ typingBy: '', isTyping: false });

  commonService = inject(CommonService);
  alertService = inject(AlertService);
  authenticationService = inject(AuthenticationService);
  environmentService = inject(EnvironmentService);
  private connection!: signalR.HubConnection;

  currentUserId = this.authenticationService.getUser()?.uid ?? '';

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
        'ReciveTypingRequest',
        (typingBy: string, isTyping: boolean, messageTime: string) => {
          this.isUserTyping.set({ typingBy, isTyping });
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

      this.connection.on('ConnectedUser', (users: any) => {
        const modifiedUsers = users.map((user: any) => ({
          ...user,
          time: this.commonService.formatTime(user.date),
        }));
        this.addChatUsers(modifiedUsers);
      });

      this.setMessage();
    }, 3000);
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

  addChatUsers(incomingChatUsers: IChatUser[]): void {
    incomingChatUsers = incomingChatUsers.filter(
      (x) => x.userId !== this.currentUserId,
    );

    if (this.chatUsers().length === 0) {
      this.chatUsers.set(incomingChatUsers);
    } else {
      incomingChatUsers.forEach((incomingUser) => {
        const existingUserIndex = this.chatUsers().findIndex(
          (user) => user.userId === incomingUser.userId,
        );

        if (existingUserIndex === -1) {
          this.chatUsers.update((items) => [...items, incomingUser]);
        }
      });

      this.chatUsers.update((items) =>
        items.filter((item) =>
          incomingChatUsers.some(
            (incomingUser) => incomingUser.userId === item.userId,
          ),
        ),
      );
    }
  }

  setMessage() {
    this.connection.on(
      'ReceiveChatMessage',
      (messageObj: IChatMessage, messageTime: any) => {
        const userIndex = this.chatUsers().findIndex(
          (user: IChatUser) => user.userId === messageObj.toUserId,
        );

        if (userIndex !== -1) {
          this.chatUsers.update((users) => {
            const updatedUsers = [...users];
            const userToUpdate = { ...updatedUsers[userIndex] };

            if (
              !userToUpdate.messages ||
              !Array.isArray(userToUpdate.messages)
            ) {
              userToUpdate.messages = [];
            }

            userToUpdate.messages.push(messageObj);
            userToUpdate.preview = messageObj.text;

            updatedUsers[userIndex] = userToUpdate;

            this.removeNotification(updatedUsers[userIndex].userId);
            let taskObj = {
              id: updatedUsers[userIndex].userId,
              message: `<strong class="text-sm mr-1">New message</strong> from <strong>${updatedUsers[userIndex].name}</strong>: ${messageObj.text}`,
              time: this.commonService.formatCustomDateTime(messageTime),
              routePath: '/' + appPaths.chat,
            } as INotificationMessage;
            this.addNotification(taskObj);

            return updatedUsers;
          });
        }
      },
    );
  }

  public async sendTypingRequest(
    userId: string,
    typingBy: string,
    isTyping: boolean,
  ) {
    return this.connection.invoke('UserTyping', userId, typingBy, isTyping);
  }
}
