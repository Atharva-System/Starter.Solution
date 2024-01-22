import { trigger, transition, style, animate } from '@angular/animations';
import { Component, ViewChild, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { NgScrollbar, NgScrollbarModule } from 'ngx-scrollbar';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MenuModule } from 'headlessui-angular';
import { UserService } from '../../services/user.service';
import { SendReciveNotificationService } from '../../../../core/services/send-recive-notification.service';
import { CommonService } from '../../../../core/services/common.service';
import { AuthenticationService } from '../../../../core/services/authentication.service';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [NgClass, FormsModule, NgScrollbarModule, MenuModule],
  animations: [
    trigger('toggleAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'scale(0.95)' }),
        animate('100ms ease-out', style({ opacity: 1, transform: 'scale(1)' })),
      ]),
      transition(':leave', [
        animate('75ms', style({ opacity: 0, transform: 'scale(0.95)' })),
      ]),
    ]),
  ],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css',
})
export class ChatComponent {
  designation = '';

  userService = inject(UserService);
  commonService = inject(CommonService);
  authenticationService = inject(AuthenticationService);
  sendReciveNotificationService = inject(SendReciveNotificationService);

  profileFromSignal = this.userService.profileSignal;
  contactList = this.sendReciveNotificationService.chatUsers;
  isUserTyping = this.sendReciveNotificationService.isUserTyping;
  connectedUserIds = this.sendReciveNotificationService.connectedUserIds;

  constructor(public storeData: Store<any>) {
    this.initStore();
    this.designation = this.authenticationService.getUser()?.roles ?? '';
  }
  store: any;
  async initStore() {
    this.storeData
      .select((d) => d.index)
      .subscribe((d) => {
        this.store = d;
      });
  }
  @ViewChild('scrollable') scrollable!: NgScrollbar;
  isShowUserChat = false;
  isShowChatMenu = false;

  searchUser = '';
  textMessage = '';
  selectedUser: any = null;

  searchUsers() {
    const contactList = this.contactList();

    if (!contactList || typeof this.searchUser !== 'string') {
      return [];
    }
    return contactList.filter((d: { name: string }) => {
      return (
        d.name && d.name.toLowerCase().includes(this.searchUser.toLowerCase())
      );
    });
  }

  selectUser(user: any) {
    if (user.messages != null) {
      user.messages.forEach((message: any) => {
        message.time = this.commonService.formatTimeAgo(message.date);
      });
    }
    this.selectedUser = user;
    this.isShowUserChat = true;
    this.scrollToBottom();
    this.isShowChatMenu = false;
  }

  sendMessage() {
    if (this.textMessage.trim()) {
      const user: any = this.contactList().find(
        (d: { userId: any }) => d.userId === this.selectedUser.userId,
      );
      if (!user.messages) {
        user.messages = [
          {
            fromUserId: this.selectedUser.userId,
            toUserId: 0,
            text: this.textMessage,
            time: 'Just now',
            date: new Date(),
          },
        ];
        user.preview = 'You: ' + this.textMessage;
      } else {
        user.messages.push({
          fromUserId: this.selectedUser.userId,
          toUserId: 0,
          text: this.textMessage,
          time: 'Just now',
          date: new Date(),
        });
        user.preview = 'You: ' + this.textMessage;
      }
      this.userService
        .sendMessage(this.selectedUser.userId, this.textMessage)
        .subscribe((data) => {});

      this.textMessage = '';
      this.scrollToBottom();
    }
    this.sendTypingRequest(false);
  }

  scrollToBottom() {
    if (this.isShowUserChat) {
      setTimeout(() => {
        this.scrollable.scrollTo({ bottom: 0 });
      });
    }
  }

  sendTypingRequest(isTyping: boolean) {
    this.sendReciveNotificationService.sendTypingRequest(
      this.selectedUser.userId,
      this.authenticationService.getUser()?.uid ?? '',
      isTyping,
    );
  }
}
