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
import { ModalComponent } from '../../../../shared/ui/modal/modal.component';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { InputComponent } from '../../../../shared/ui/input/input.component';
import { Router } from '@angular/router';
import { FieldValidation, Regex } from '../../../../shared/constants/constants';
import { authPaths } from '../../../../shared/constants/routes';
import { AlertService } from '../../../../shared/services/alert.service';
import { AuthService } from '../../../auth/services/auth.service';
import { UserService } from '../../services/user.service';
import { IUpdateUser } from '../../models/update-user.interface';

@Component({
  selector: 'app-invite-user-modal',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    InputComponent,
    ButtonComponent,
    ModalComponent,
  ],
  templateUrl: './invite-user-modal.component.html',
  styleUrl: './invite-user-modal.component.css',
})
export class InviteUserModalComponent implements OnChanges {
  @Input() userId = '';
  @Output() saved = new EventEmitter();
  @Output() discard = new EventEmitter();
  @ViewChild('modalComponent') modalComponent!: ModalComponent;
  signinRoute = '/' + authPaths.signin;
  formInviteUser!: FormGroup;
  isSubmitFormInviteUser = false;

  router = inject(Router);
  fb = inject(FormBuilder);
  alertService = inject(AlertService);
  authService = inject(AuthService);
  userService = inject(UserService);

  constructor() {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['userId']) {
      if (this.userId) {
        this.setUserDetails();
      }
    }
  }

  setUserDetails() {
    this.userService.getUser(this.userId).subscribe((data) => {
      this.formInviteUser.patchValue(data.data);
    });
  }

  open() {
    setTimeout(() => {
      this.modalComponent.open();
    }, 10);
  }

  initForm() {
    this.formInviteUser = this.fb.group({
      firstName: [
        '',
        Validators.compose([
          Validators.required,
          Validators.maxLength(FieldValidation.firstNameMaxLength),
          Validators.pattern(Regex.noSpaceValidationPattern),
        ]),
      ],
      lastName: [
        '',
        Validators.compose([
          Validators.required,
          Validators.maxLength(FieldValidation.lastNameMaxLength),
          Validators.pattern(Regex.noSpaceValidationPattern),
        ]),
      ],
      email: [
        '',
        Validators.compose([
          Validators.required,
          Validators.email,
          Validators.maxLength(FieldValidation.emailMaxLength),
        ]),
      ],
    });
  }

  resetForm() {
    this.isSubmitFormInviteUser = false;
    this.formInviteUser.reset();
    this.discard.emit();
  }

  submitForm() {
    this.isSubmitFormInviteUser = true;
    if (this.formInviteUser.valid) {
      const formValues = this.formInviteUser.value;
      if (this.userId) {
        const updateUserDto: IUpdateUser = {
          id: this.userId,
          firstName: formValues.firstName,
          lastName: formValues.lastName,
          email: formValues.email,
        };
        this.userService.updateUsers(updateUserDto).subscribe(
          (res: any) => {
            this.isSubmitFormInviteUser = false;
            this.alertService.showMessage('User updated successfully.');
            this.formInviteUser.reset();
            this.modalComponent.close();
            this.saved.emit();
          },
          (error) => {},
        );
      } else {
        this.authService.inviteUser(formValues).subscribe(
          (res: any) => {
            this.isSubmitFormInviteUser = false;
            this.alertService.showMessage('User invited successfully.');
            this.formInviteUser.reset();
            this.modalComponent.close();
            this.saved.emit();
          },
          (error) => {},
        );
      }
    }
  }
}
