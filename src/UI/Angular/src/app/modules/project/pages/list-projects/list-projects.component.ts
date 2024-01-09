import { NgClass, NgStyle } from '@angular/common';
import { Component, ViewChild, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DataTableModule, colDef } from '@bhplugin/ng-datatable';
import { PaginationFilter } from '../../../../core/models/pagination-filter.interface';
import { FilterService } from '../../../../core/services/filter.service';
import { AlertService } from '../../../../shared/services/alert.service';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { DeleteConfirmationModalComponent } from '../../../../shared/ui/delete-confirmation-modal/delete-confirmation-modal.component';
import { ManageProjectModalComponent } from '../../components/manage-project-modal/manage-project-modal.component';
import { ProjectService } from '../../services/project.service';
import { MenuModule } from 'headlessui-angular';
import {
  DropdownComponent,
  IDropdownItems,
} from '../../../../shared/ui/dropdown/dropdown.component';
import { DateRangePickerComponent } from '../../../../shared/ui/date-range-picker/date-range-picker.component';

@Component({
  selector: 'app-list-projects',
  standalone: true,
  imports: [
    NgClass,
    NgStyle,
    DataTableModule,
    FormsModule,
    ButtonComponent,
    ManageProjectModalComponent,
    DeleteConfirmationModalComponent,
    MenuModule,
    DropdownComponent,
    DateRangePickerComponent,
  ],
  templateUrl: './list-projects.component.html',
  styleUrl: './list-projects.component.css',
})
export class ListProjectsComponent {
  @ViewChild('manageProjectModalComponent')
  manageProjectModalComponent!: ManageProjectModalComponent;
  @ViewChild('deleteProjectModal')
  deleteProjectModal!: DeleteConfirmationModalComponent;
  search = '';
  searchDates: { from: any; to: any } = {
    from: '',
    to: '',
  };

  projectService = inject(ProjectService);
  filterService = inject(FilterService);
  alertService = inject(AlertService);
  timer: any;
  deleteProjectId = '';
  editProjectId = '';
  selectedFilterDropdownField = 'projectName';
  searchBoxType = 'text';
  params: PaginationFilter;

  loading: boolean = true;
  cols: Array<colDef> = [
    { field: 'projectName', title: 'Project Name' },
    { field: 'startDateDisplay', title: 'Start Date' },
    { field: 'endDateDisplay', title: 'End Date' },
    { field: 'estimatedHours', title: 'Estimated Hours' },
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
      text: 'Project Name',
      value: 'projectName',
      selected: true,
    },
    { text: 'Start Date', value: 'startDate' },
    { text: 'End Date', value: 'endDate' },
    { text: 'Estimated Hours', value: 'estimatedHours' },
  ];

  rows: Array<any> = [];
  total_rows: number = 0;

  constructor() {
    this.params = { ...this.filterService.defaultFilter };
    this.getProject();
  }

  selectFilterDropdown(field: string) {
    if (field == 'estimatedHours') {
      this.searchBoxType = 'number';
    } else if (field == 'startDate' || field == 'endDate') {
      this.searchBoxType = 'date';
    } else {
      this.searchBoxType = 'text';
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

  async getProject() {
    this.loading = true;
    this.projectService.getProjects(this.params).subscribe((data) => {
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
    this.getProject();
  }

  getBadgeColor(status: string): string {
    switch (status) {
      case 'Active':
        return 'badge-outline-success';
      case 'Inactive':
        return 'badge-outline-danger';
      default:
        return 'badge-outline-info';
    }
  }

  getSortColumnName(column: string): string {
    switch (column) {
      case 'startDateDisplay':
        return 'startDate';
      case 'endDateDisplay':
        return 'endDate';
      default:
        return column;
    }
  }

  openCreateProjectModal() {
    this.manageProjectModalComponent.open();
  }

  deleteProject(id: string) {
    this.deleteProjectModal.open();
    this.deleteProjectId = id;
  }

  editProject(id: string) {
    this.editProjectId = id;
    this.openCreateProjectModal();
  }

  onDelete() {
    this.projectService
      .deleteProject(this.deleteProjectId)
      .subscribe((data) => {
        this.alertService.showMessage(data.message);
        this.getProject();
        this.deleteProjectModal.close();
      });
  }

  onSave() {
    this.getProject();
    this.onCancel();
  }

  onCancel() {
    this.deleteProjectId = this.editProjectId = '';
  }

  clearSearchBox() {
    this.search = '';
    this.searchDates = {
      from: '',
      to: '',
    };
    this.params.AdvancedFilter = null;
    this.getProject();
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
        this.parsValue(this.selectedFilterDropdownField, this.search),
        this.params,
      );
      this.getProject();
      this.searchTimeout = null;
    }, 1000);
  }

  parsValue(column: string, value: any): any {
    switch (column) {
      case 'estimatedHours':
        return parseFloat(value);
      default:
        return value;
    }
  }

  getCondition(column: string): any {
    switch (column) {
      case 'projectName':
        return 'contain';
      default:
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
      this.getProject();
      this.searchDateTimeout = null;
    }, 1000);
  }

  dateRangeclear() {
    this.clearSearchBox();
  }
}
