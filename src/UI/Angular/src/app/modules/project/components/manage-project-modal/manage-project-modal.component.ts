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
import { authPaths } from '../../../../shared/constants/routes';
import { AlertService } from '../../../../shared/services/alert.service';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { InputComponent } from '../../../../shared/ui/input/input.component';
import { ModalComponent } from '../../../../shared/ui/modal/modal.component';
import { AuthService } from '../../../auth/services/auth.service';
import { ProjectService } from '../../services/project.service';
import { IUpdateProject } from '../../models/update-project.interface';
import { ICreateProject } from '../../models/create-project.interface';
import { DateRangePickerComponent } from '../../../../shared/ui/date-range-picker/date-range-picker.component';
import { CommonService } from '../../../../core/services/common.service';
import { FieldValidation, Regex } from '../../../../shared/constants/constants';
import { TextEditorComponent } from '../../../../shared/ui/text-editor/text-editor.component';

@Component({
  selector: 'app-manage-project-modal',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    InputComponent,
    ButtonComponent,
    ModalComponent,
    TextEditorComponent,
    DateRangePickerComponent,
  ],
  templateUrl: './manage-project-modal.component.html',
  styleUrl: './manage-project-modal.component.css',
})
export class ManageProjectModalComponent implements OnChanges {
  @Input() projectId = '';
  @Output() saved = new EventEmitter();
  @Output() discard = new EventEmitter();
  @ViewChild('modalComponent') modalComponent!: ModalComponent;
  signinRoute = '/' + authPaths.signin;
  formCreateProject!: FormGroup;
  isSubmitFormCreateProject = false;

  commonService = inject(CommonService);
  router = inject(Router);
  fb = inject(FormBuilder);
  alertService = inject(AlertService);
  authService = inject(AuthService);
  projectService = inject(ProjectService);

  constructor() {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['projectId']) {
      if (this.projectId) {
        this.setProjectDetails();
      }
    }
  }

  setProjectDetails() {
    this.projectService.getProject(this.projectId).subscribe((data) => {
      const startDate = new Date(data.data.startDate);
      const endDate = new Date(data.data.endDate);

      this.formCreateProject.patchValue({
        projectName: data.data.projectName,
        description: data.data.description,
        deadline: {
          from: this.commonService.getUTCDate(startDate),
          to: this.commonService.getUTCDate(endDate),
        },
        estimatedHours: data.data.estimatedHours,
      });
    });
  }

  open() {
    setTimeout(() => {
      this.modalComponent.open();
    }, 10);
  }

  initForm() {
    this.formCreateProject = this.fb.group({
      projectName: [
        '',
        Validators.compose([
          Validators.required,
          Validators.maxLength(FieldValidation.projectNameMaxLength),
        ]),
      ],
      description: [
        '',
        Validators.compose([
          Validators.maxLength(FieldValidation.descriptionMaxLength),
        ]),
      ],
      deadline: ['', Validators.compose([Validators.required])],
      estimatedHours: [
        '',
        Validators.compose([
          Validators.required,
          Validators.pattern(Regex.decimalValidationPattern),
          Validators.maxLength(FieldValidation.estimatedHoursMaxLength),
        ]),
      ],
    });
  }

  resetForm() {
    this.isSubmitFormCreateProject = false;
    this.formCreateProject.reset();
    this.discard.emit();
  }

  submitForm() {
    this.isSubmitFormCreateProject = true;
    if (this.formCreateProject.valid) {
      const formValues = this.formCreateProject.value;
      if (this.projectId) {
        const updateProjectDto: IUpdateProject = {
          id: this.projectId,
          projectName: formValues.projectName,
          description: formValues.description,
          startDate: formValues.deadline.from,
          endDate: formValues.deadline.to,
          estimatedHours: formValues.estimatedHours,
        };
        this.projectService.updateProject(updateProjectDto).subscribe(
          (res: any) => {
            this.isSubmitFormCreateProject = false;
            this.alertService.showMessage('Project updated successfully.');
            this.formCreateProject.reset();
            this.modalComponent.close();
            this.saved.emit();
          },
          (error) => {},
        );
      } else {
        const createProjectDto: ICreateProject = {
          projectName: formValues.projectName,
          description: formValues.description,
          startDate: formValues.deadline.from,
          endDate: formValues.deadline.to,
          estimatedHours: formValues.estimatedHours,
        };
        this.projectService.createProject(createProjectDto).subscribe(
          (res: any) => {
            this.isSubmitFormCreateProject = false;
            this.alertService.showMessage('Project Created successfully.');
            this.formCreateProject.reset();
            this.modalComponent.close();
            this.saved.emit();
          },
          (error) => {},
        );
      }
    }
  }
}
