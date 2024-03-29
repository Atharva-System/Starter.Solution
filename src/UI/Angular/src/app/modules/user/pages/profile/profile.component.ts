import { NgClass } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { UserService } from '../../services/user.service';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { FieldValidation, Regex } from '../../../../shared/constants/constants';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { InputComponent } from '../../../../shared/ui/input/input.component';
import { AlertService } from '../../../../shared/services/alert.service';
import {
  IUserProfile,
  IUserProfileSignal,
} from '../../models/user-profile.interface';
import { AuthenticationService } from '../../../../core/services/authentication.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [NgClass, InputComponent, ButtonComponent, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css',
})
export class ProfileComponent implements OnInit {
  userProfileSignal!: IUserProfileSignal;
  formProfile!: FormGroup;
  isSubmitFormProfile = false;
  isFormDirty = false;
  originalData = '';
  profileEmail = '';

  authenticationService = inject(AuthenticationService);
  userService = inject(UserService);
  fb = inject(FormBuilder);
  alertService = inject(AlertService);

  constructor() {
    this.initForm();
  }

  initForm() {
    this.formProfile = this.fb.group({
      id: '',
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
    });
  }

  ngOnInit(): void {
    this.setProfileDetails();
  }

  setProfileDetails() {
    this.userService.getProfileDetail().subscribe((data) => {
      this.formProfile.patchValue(data.data);
      this.userProfileSignal = {
        email: data.data?.email,
        name: data.data?.firstName + ' ' + data.data?.lastName,
      };
      this.profileEmail = data.data?.email;
      this.userService.setProfileSignal(this.userProfileSignal);
      this.originalData = JSON.stringify(this.formProfile.value);
      this.isFormDirty = false;
      this.formProfile.valueChanges.subscribe(() => {
        this.checkFormDirty();
      });
    });
  }

  submitFormProfile() {
    this.isSubmitFormProfile = true;
    if (this.formProfile.valid) {
      const formValues = this.formProfile.value as IUserProfile;
      formValues.email = this.profileEmail;
      this.userService.updateProfile(formValues).subscribe(
        (res: any) => {
          this.userProfileSignal = {
            email: this.profileEmail,
            name: formValues.firstName + ' ' + formValues.lastName,
          };
          this.userService.setProfileSignal(this.userProfileSignal);
          this.authenticationService.updateStorageUserInfo(
            this.userProfileSignal.name,
            this.userProfileSignal.email,
          );
          this.isSubmitFormProfile = false;
          this.originalData = JSON.stringify(this.formProfile.value);
          this.isFormDirty = false;
          this.alertService.showMessage(res.data);
        },
        (error) => {},
      );
    }
  }

  checkFormDirty() {
    this.isFormDirty =
      JSON.stringify(this.formProfile.value) !== this.originalData;
  }
}
