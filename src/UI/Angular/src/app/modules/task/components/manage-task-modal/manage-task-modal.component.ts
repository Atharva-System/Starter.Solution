import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
  ViewChild,
  inject,
} from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  FormGroup,
  FormBuilder,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService } from '../../../../core/services/common.service';
import { FieldValidation } from '../../../../shared/constants/constants';
import { authPaths } from '../../../../shared/constants/routes';
import { AlertService } from '../../../../shared/services/alert.service';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { DateRangePickerComponent } from '../../../../shared/ui/date-range-picker/date-range-picker.component';
import { InputComponent } from '../../../../shared/ui/input/input.component';
import { ModalComponent } from '../../../../shared/ui/modal/modal.component';
import { AuthService } from '../../../auth/services/auth.service';
import { TaskService } from '../../services/task.service';
import { IUpdateTask } from '../../models/update-task.interface';
import { ICreateTask } from '../../models/create-task.interface';
import {
  ISelectItems,
  SelectComponent,
} from '../../../../shared/ui/select/select.component';
import { PaginationFilter } from '../../../../core/models/pagination-filter.interface';
import { TextEditorComponent } from '../../../../shared/ui/text-editor/text-editor.component';

@Component({
  selector: 'app-manage-task-modal',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    InputComponent,
    ButtonComponent,
    ModalComponent,
    DateRangePickerComponent,
    SelectComponent,
    TextEditorComponent,
  ],
  templateUrl: './manage-task-modal.component.html',
  styleUrl: './manage-task-modal.component.css',
})
export class ManageTaskModalComponent implements OnChanges {
  @Input() taskId = '';
  @Output() saved = new EventEmitter();
  @Output() discard = new EventEmitter();
  @ViewChild('modalComponent') modalComponent!: ModalComponent;
  priorityOptions: ISelectItems[] = [];
  statusOptions: ISelectItems[] = [];
  projectsOptions: ISelectItems[] = [];
  assignToUsersOptions: ISelectItems[] = [];
  signinRoute = '/' + authPaths.signin;
  formCreateTask!: FormGroup;
  isSubmitFormCreateTask = false;
  isFormDirty = false;
  originalData = '';

  commonService = inject(CommonService);
  router = inject(Router);
  fb = inject(FormBuilder);
  alertService = inject(AlertService);
  authService = inject(AuthService);
  taskService = inject(TaskService);
  params!: PaginationFilter;

  constructor() {
    this.initForm();
    this.bindStatus();
    this.bindPriority();
    this.bindProjects();
    this.bindAssignToUsers();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['taskId']) {
      if (this.taskId) {
        this.setTaskDetails();
      }
    }
  }

  bindProjects() {
    this.taskService.getProjects().subscribe((data) => {
      this.projectsOptions = data.data.map(
        ({ id, projectName }: { id: any; projectName: string }) => ({
          value: `${id}`,
          label: projectName,
        }),
      );
    });
  }

  bindAssignToUsers() {
    this.taskService.getAssignees().subscribe((data) => {
      this.assignToUsersOptions = data.data.map(
        ({ id, name }: { id: any; name: string }) => ({
          value: `${id}`,
          label: name,
        }),
      );
    });
  }

  bindStatus() {
    this.taskService.getTaskStatusList().subscribe((data) => {
      this.statusOptions = data.data.map(
        ({ id, displayName }: { id: any; displayName: string }) => ({
          value: `${id}`,
          label: displayName,
        }),
      );
    });
  }

  bindPriority() {
    this.taskService.getTaskPriorityList().subscribe((data) => {
      this.priorityOptions = data.data.map(
        ({ id, displayName }: { id: any; displayName: string }) => ({
          value: `${id}`,
          label: displayName,
        }),
      );
    });
  }

  setTaskDetails() {
    this.taskService.getTask(this.taskId).subscribe((data) => {
      const startDate = new Date(data.data.startDate);
      const endDate = new Date(data.data.endDate);

      this.formCreateTask.patchValue({
        taskName: data.data.taskName,
        description: data.data.description,
        deadline: {
          from: this.commonService.getUTCDate(startDate),
          to: this.commonService.getUTCDate(endDate),
        },
        status: `${data.data.status}`,
        priority: `${data.data.priority}`,
        projectId: data.data.projectId,
        assignedTo: data.data.assignedTo,
      });
      this.originalData = JSON.stringify(this.formCreateTask.value);
      this.isFormDirty = false;
      this.formCreateTask.valueChanges.subscribe(() => {
        this.checkFormDirty();
      });
    });
  }

  open() {
    setTimeout(() => {
      this.modalComponent.open();
    }, 10);
  }

  initForm() {
    this.formCreateTask = this.fb.group({
      taskName: [
        '',
        Validators.compose([
          Validators.required,
          Validators.maxLength(FieldValidation.taskNameMaxLength),
        ]),
      ],
      description: [
        '',
        Validators.compose([
          Validators.maxLength(FieldValidation.descriptionMaxLength),
        ]),
      ],
      deadline: ['', Validators.compose([Validators.required])],
      status: ['', Validators.compose([Validators.required])],
      priority: ['', Validators.compose([Validators.required])],
      projectId: ['', Validators.compose([Validators.required])],
      assignedTo: ['', Validators.compose([Validators.required])],
    });
  }

  resetForm() {
    this.isSubmitFormCreateTask = false;
    this.formCreateTask.reset();
    this.discard.emit();
  }

  submitForm(doClose: boolean = true) {
    this.isSubmitFormCreateTask = true;
    if (this.formCreateTask.valid) {
      const formValues = this.formCreateTask.value;
      if (this.taskId) {
        const updateTaskDto: IUpdateTask = {
          id: this.taskId,
          taskName: formValues.taskName,
          description: formValues.description,
          startDate: formValues.deadline.from,
          endDate: formValues.deadline.to,
          status: formValues.status,
          priority: formValues.priority,
          projectId: formValues.projectId,
          assignedTo: formValues.assignedTo,
        };
        this.taskService.updateTask(updateTaskDto).subscribe(
          (res: any) => {
            this.isSubmitFormCreateTask = false;
            this.alertService.showMessage('Task updated successfully.');
            this.formCreateTask.reset();
            this.modalComponent.close();
            this.saved.emit();
          },
          (error) => {},
        );
      } else {
        const createTaskDto: ICreateTask = {
          taskName: formValues.taskName,
          description: formValues.description,
          startDate: formValues.deadline.from,
          endDate: formValues.deadline.to,
          status: formValues.status,
          priority: formValues.priority,
          projectId: formValues.projectId,
          assignedTo: formValues.assignedTo,
        };

        this.taskService.createTask(createTaskDto).subscribe(
          (res: any) => {
            this.isSubmitFormCreateTask = false;
            this.alertService.showMessage('Task Created successfully.');
            this.formCreateTask.reset();
            if (doClose) this.modalComponent.close();
            this.saved.emit();
          },
          (error) => {},
        );
      }
    }
  }

  checkFormDirty() {
    this.isFormDirty =
      JSON.stringify(this.formCreateTask.value) !== this.originalData;
  }
}
