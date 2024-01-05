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
  ],
  templateUrl: './list-projects.component.html',
  styleUrl: './list-projects.component.css',
})
export class ListProjectsComponent {
  @ViewChild('manageProjectModalComponent')
  manageProjectModalComponent!: ManageProjectModalComponent;
  @ViewChild('deleteProjectModal')
  deleteProjectModal!: DeleteConfirmationModalComponent;

  projectService = inject(ProjectService);
  filterService = inject(FilterService);
  alertService = inject(AlertService);
  timer: any;
  deleteProjectId = '';
  editProjectId = '';

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

  rows: Array<any> = [];
  total_rows: number = 0;

  constructor() {
    this.params = { ...this.filterService.defaultFilter };
    this.getProject();
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
    this.params = this.filterService.generateFilter(
      data.column_filters,
      this.params,
    );
    if (data.change_type === 'filter') {
      this.filterProjects();
    } else {
      this.getProject();
    }
  }

  filterProjects() {
    clearTimeout(this.timer);
    this.timer = setTimeout(() => {
      this.getProject();
    }, 300);
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
}
