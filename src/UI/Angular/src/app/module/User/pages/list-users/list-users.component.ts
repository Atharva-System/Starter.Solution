import { NgClass, NgStyle } from '@angular/common';
import { Component, ViewChild, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DataTableModule, colDef } from '@bhplugin/ng-datatable';
import { UserService } from '../../services/user.service';
import { PaginationFilter } from '../../../../core/models/pagination-filter.interface';
import { CommonFilterService } from '../../../../core/services/common-filter.service';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { InviteUserModalComponent } from '../../components/invite-user-modal/invite-user-modal.component';
import { DeleteConfirmationModalComponent } from '../../../../shared/ui/delete-confirmation-modal/delete-confirmation-modal.component';
import { AlertService } from '../../../../shared/services/alert.service';

@Component({
  selector: 'app-list-users',
  standalone: true,
  imports: [
    NgClass,
    NgStyle,
    DataTableModule,
    FormsModule,
    ButtonComponent,
    InviteUserModalComponent,
    DeleteConfirmationModalComponent,
  ],
  templateUrl: './list-users.component.html',
  styleUrl: './list-users.component.css',
})
export class ListUsersComponent {
  @ViewChild('inviteUserModal') inviteUserModal!: InviteUserModalComponent;
  @ViewChild('deleteUserModal')
  deleteUserModal!: DeleteConfirmationModalComponent;

  userService = inject(UserService);
  commonFilterService = inject(CommonFilterService);
  alertService = inject(AlertService);
  timer: any;
  deleteUserId = '';
  editUserId = '';

  params: PaginationFilter;

  loading: boolean = true;
  cols: Array<colDef> = [
    { field: 'fullName', title: 'Full Name' },
    { field: 'email', title: 'Email' },
    { field: 'status', title: 'Status' },
    { field: 'role', title: 'Role' },
    {
      field: 'action',
      title: 'Action',
      sort: false,
      filter: false,
      headerClass: 'justify-center',
    },
  ];

  rows: Array<any> = [];
  total_rows: number = 0;

  constructor() {
    this.params = { ...this.commonFilterService.defaultFilter };
    this.getUsers();
  }

  async getUsers() {
    this.loading = true;
    this.userService.getUsers(this.params).subscribe((data) => {
      this.rows = data?.data;
      this.total_rows = data?.totalCount;
      this.loading = false;
    });
  }

  changeServer(data: any) {
    this.params.PageNumber = data.current_page;
    this.params.PageSize = data.pagesize;
    this.params.OrderBy = [data.sort_column + ' ' + data.sort_direction];
    this.params = this.commonFilterService.generateFilter(
      data.column_filters,
      this.params,
    );
    if (data.change_type === 'filter') {
      this.filterUsers();
    } else {
      this.getUsers();
    }
  }

  filterUsers() {
    clearTimeout(this.timer);
    this.timer = setTimeout(() => {
      this.getUsers();
    }, 300);
  }

  getBadgeColor(status: string): string {
    switch (status) {
      case 'Invited':
        return 'badge-outline-info';
      case 'Active':
        return 'badge-outline-success';
      case 'Inactive':
        return 'badge-outline-danger';
      default:
        return 'badge-outline-info';
    }
    return '';
  }

  openInviteUserModal() {
    this.inviteUserModal.open();
  }

  deleteUser(id: string) {
    this.deleteUserModal.open();
    this.deleteUserId = id;
  }

  editUser(id: string) {
    this.editUserId = id;
    this.openInviteUserModal();
  }

  onDelete() {
    this.userService.deleteUser(this.deleteUserId).subscribe((data) => {
      this.alertService.showMessage(data.message);
      this.getUsers();
      this.deleteUserModal.close();
    });
  }

  onCancel() {
    this.deleteUserId = this.editUserId = '';
  }
}
