import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output, ViewChild, inject } from '@angular/core';
import { ModalComponent } from '../../../../shared/ui/modal/modal.component';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { InputComponent } from '../../../../shared/ui/input/input.component';
import { Router } from '@angular/router';
import { FieldValidation } from '../../../../shared/constants/constants';
import { authPaths } from '../../../../shared/constants/routes';
import { AlertService } from '../../../../shared/services/alert.service';
import { AuthService } from '../../../auth/services/auth.service';

@Component({
  selector: 'app-invite-user-modal',
  standalone: true,
  imports: [FormsModule,
    ReactiveFormsModule,
    InputComponent,
    ButtonComponent,ModalComponent,CommonModule],
  templateUrl: './invite-user-modal.component.html',
  styleUrl: './invite-user-modal.component.css'
})
export class InviteUserModalComponent {
  @Output()  saved = new EventEmitter()
  @ViewChild('modalComponent') modalComponent!: ModalComponent;
  signinRoute = '/' + authPaths.signin;
  formInviteUser!: FormGroup;
  isSubmitFormInviteUser = false;

  router = inject(Router);
  fb = inject(FormBuilder);
  alertService = inject(AlertService);
  authService = inject(AuthService);

  constructor() {
    this.initForm();
  }

  open(){
    this.modalComponent.open()
  }

  initForm() {
    this.formInviteUser = this.fb.group({
      firstName: [
        '',
        Validators.compose([
          Validators.required,
          Validators.maxLength(FieldValidation.firstNameMaxLength),
        ]),
      ],
      lastName: [
        '',
        Validators.compose([
          Validators.required,
          Validators.maxLength(FieldValidation.lastNameMaxLength),
        ]),
      ],
      email: [
        '',
        Validators.compose([
          Validators.required,
          Validators.email,
          Validators.maxLength(FieldValidation.emailMaxLength),
        ]),
      ]
    });
  }

  resetForm(){
    this.isSubmitFormInviteUser = false;
    this.formInviteUser.reset();
  }

  submitForm() {
    this.isSubmitFormInviteUser = true;
    if (this.formInviteUser.valid) {
      const formValues = this.formInviteUser.value;
      this.authService.inviteUser(formValues).subscribe(
        (res: any) => {
          this.isSubmitFormInviteUser = false;
          this.alertService.showMessage('User invited successfully.');
          this.formInviteUser.reset();
          this.modalComponent.close()
          this.saved.emit()
        },
        (error) => {},
      );
    }
  }
}

