import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { authPaths } from '../../../../shared/constants/routes';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AlertService } from '../../../../shared/services/alert.service';
import { InputComponent } from '../../../../shared/ui/input/input.component';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { AnchorComponent } from '../../../../shared/ui/anchor/anchor.component';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.css',
  standalone: true,
  imports: [RouterModule,FormsModule , ReactiveFormsModule, InputComponent, ButtonComponent,AnchorComponent],
})
export class SigninComponent {
  signupRoute = '/' + authPaths.signup;
  formSignin!: FormGroup;
  isSubmitFormSignin = false;

  constructor(
    public router: Router,
    public fb: FormBuilder,
    public alertService: AlertService,
  ) {
    this.initForm();
  }

  initForm() {
    this.formSignin = this.fb.group({
      email: ['', Validators.compose([Validators.required, Validators.email])],
      password: ['', Validators.required],
    });
  }

  submitFormSignin() {
    this.isSubmitFormSignin = true;
    if (this.formSignin.valid) {
      const formValues = this.formSignin.value;
      console.log('Form Values:', formValues);
      this.alertService.showMessage('Form submitted successfully.');
    }
  }
}
