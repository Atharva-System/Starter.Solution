import { NgClass, NgStyle } from '@angular/common';
import { Component, ViewChild, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DataTableModule, colDef } from '@bhplugin/ng-datatable';
import { MenuModule } from 'headlessui-angular';
import { PaginationFilter } from '../../../../core/models/pagination-filter.interface';
import { FilterService } from '../../../../core/services/filter.service';
import { AlertService } from '../../../../shared/services/alert.service';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { DateRangePickerComponent } from '../../../../shared/ui/date-range-picker/date-range-picker.component';
import { DeleteConfirmationModalComponent } from '../../../../shared/ui/delete-confirmation-modal/delete-confirmation-modal.component';
import {
  DropdownComponent,
  IDropdownItems,
} from '../../../../shared/ui/dropdown/dropdown.component';
import { ManageTaskModalComponent } from '../../components/manage-task-modal/manage-task-modal.component';
import { TaskService } from '../../services/task.service';

@Component({
  selector: 'app-list-tasks',
  standalone: true,
  imports: [
    NgClass,
    NgStyle,
    DataTableModule,
    FormsModule,
    ButtonComponent,
    ManageTaskModalComponent,
    DeleteConfirmationModalComponent,
    MenuModule,
    DropdownComponent,
    DateRangePickerComponent,
  ],
  templateUrl: './list-tasks.component.html',
  styleUrl: './list-tasks.component.css',
})
export class ListTasksComponent {
  @ViewChild('manageTaskModalComponent')
  manageTaskModalComponent!: ManageTaskModalComponent;
  @ViewChild('deleteTaskModal')
  deleteTaskModal!: DeleteConfirmationModalComponent;
  search = '';
  searchDates: { from: any; to: any } = {
    from: '',
    to: '',
  };

  taskService = inject(TaskService);
  filterService = inject(FilterService);
  alertService = inject(AlertService);
  timer: any;
  deleteTaskId = '';
  editTaskId = '';
  selectedFilterDropdownField = 'taskName';
  searchBoxType = 'text';
  params: PaginationFilter;

  loading: boolean = true;
  cols: Array<colDef> = [
    { field: 'taskName', title: 'Task' },
    { field: 'projectName', title: 'Project' },
    { field: 'startDateDisplay', title: 'Start Date' },
    { field: 'endDateDisplay', title: 'End Date' },
    { field: 'statusDisplay', title: 'Status' },
    { field: 'priorityDisplay', title: 'Priority' },
    {
      field: 'action',
      title: 'Action',
      sort: false,
      filter: false,
      headerClass: 'justify-center',
    },
  ];

  filterDropdownDs: IDropdownItems[] = [
    {
      text: 'Task',
      value: 'taskName',
      selected: true,
    },
    {
      text: 'Project',
      value: 'projectName',
    },
    { text: 'Start Date', value: 'startDate' },
    { text: 'End Date', value: 'endDate' },
    { text: 'Status', value: 'statusDisplay' },
    { text: 'Priority', value: 'priorityDisplay' },
  ];

  rows: Array<any> = [];
  total_rows: number = 0;

  constructor() {
    this.params = { ...this.filterService.defaultFilter };
    this.getTasks();
  }

  selectFilterDropdown(field: string) {
    if (field == 'startDate' || field == 'endDate') {
      this.searchBoxType = 'date';
    } else {
      this.searchBoxType = 'string';
    }
    this.selectedFilterDropdownField = field;
    if (
      this.search != '' ||
      this.searchDates.from != '' ||
      this.searchDates.to != ''
    ) {
      this.clearSearchBox();
    }
  }

  async getTasks() {
    this.loading = true;
    this.taskService.getTasks(this.params).subscribe((data) => {
      this.rows = data?.data;
      this.total_rows = data?.totalCount;
      this.loading = false;
    });
  }

  changeServer(data: any) {
    this.params.PageNumber = data.current_page;
    this.params.PageSize = data.pagesize;
    this.params.OrderBy = [
      this.getSortColumnName(data.sort_column) + ' ' + data.sort_direction,
    ];
    this.getTasks();
  }

  getBadgeColor(status: string): string {
    switch (status) {
      case 'High':
        return 'badge-outline-success';
      case 'Low':
        return 'badge-outline-danger';
      case 'To Do':
        return 'bg-secondary';
      case 'Completed':
        return 'bg-success';
      case 'In Progress':
        return 'bg-primary';
      default:
        return 'badge-outline-info';
    }
  }

  getSortColumnName(column: string): string {
    switch (column) {
      case 'statusDisplay':
        return 'status';
      case 'priorityDisplay':
        return 'priority';
      case 'startDateDisplay':
        return 'startDate';
      case 'endDateDisplay':
        return 'endDate';
      default:
        return column;
    }
  }

  openCreateTaskModal() {
    this.manageTaskModalComponent.open();
  }

  deleteTask(id: string) {
    this.deleteTaskModal.open();
    this.deleteTaskId = id;
  }

  editTask(id: string) {
    this.editTaskId = id;
    this.openCreateTaskModal();
  }

  onDelete() {
    this.taskService.deleteTask(this.deleteTaskId).subscribe((data) => {
      this.alertService.showMessage(data.message);
      this.getTasks();
      this.deleteTaskModal.close();
    });
  }

  onSave() {
    this.getTasks();
    this.onCancel();
  }

  onCancel() {
    this.deleteTaskId = this.editTaskId = '';
  }

  clearSearchBox() {
    this.search = '';
    this.searchDates = {
      from: '',
      to: '',
    };
    this.params.AdvancedFilter = null;
    this.getTasks();
  }

  private searchTimeout: any;
  onSearch() {
    if (this.searchTimeout) {
      clearTimeout(this.searchTimeout);
    }
    this.searchTimeout = setTimeout(() => {
      this.params = this.filterService.generateSingleFilter(
        this.getCondition(this.selectedFilterDropdownField),
        this.selectedFilterDropdownField,
        this.search,
        this.params,
      );
      this.getTasks();
      this.searchTimeout = null;
    }, 1000);
  }

  getCondition(column: string): any {
    const containsColumns = [
      'taskName',
      'projectName',
      'priorityDisplay',
      'statusDisplay',
    ];

    if (containsColumns.includes(column)) {
      return 'contain';
    } else {
      return 'equal';
    }
  }

  private searchDateTimeout: any;
  dateRangeSelect(dates: any) {
    if (!dates || !dates.from || !dates.to) return;
    var fromDate = dates.from.toISOString().substring(0, 10);
    var toDate = dates.to.toISOString().substring(0, 10);
    this.searchDates = dates;
    if (this.searchDateTimeout) {
      clearTimeout(this.searchDateTimeout);
    }
    this.searchDateTimeout = setTimeout(() => {
      this.params = this.filterService.dateRangeSingleFilter(
        {
          start: this.selectedFilterDropdownField,
          to: this.selectedFilterDropdownField,
        },
        { start: fromDate, to: toDate },
        this.params,
      );
      this.getTasks();
      this.searchDateTimeout = null;
    }, 1000);
  }

  dateRangeclear() {
    this.clearSearchBox();
  }
}
