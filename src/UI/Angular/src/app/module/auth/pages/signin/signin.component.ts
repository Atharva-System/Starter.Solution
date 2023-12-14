import { Component, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { authPaths } from '../../../../shared/constants/routes';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { InputComponent } from '../../../../shared/ui/input/input.component';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { AnchorComponent } from '../../../../shared/ui/anchor/anchor.component';
import { AuthService } from '../../services/auth.service';
import { AuthenticationService } from '../../../../core/services/authentication.service';
import { IAuthenticationResponse } from '../../models/authentication-response.interface';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.css',
  standalone: true,
  imports: [
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    InputComponent,
    ButtonComponent,
    AnchorComponent,
  ],
})
export class SigninComponent {
  signupRoute = '/' + authPaths.signup;
  formSignin!: FormGroup;
  isSubmitFormSignin = false;

  router = inject(Router);
  fb = inject(FormBuilder);
  authService = inject(AuthService);
  authenticationService = inject(AuthenticationService);

  constructor() {
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
      this.authService.signin(formValues).subscribe(
        (res: any) => {
          let response = res as IAuthenticationResponse;
          this.isSubmitFormSignin = false;
          this.authenticationService.setToken(response.token);
          this.authenticationService.setRefreshToken(response.refreshToken);
          this.router.navigate(['/']);
        },
        (error) => {},
      );
    }
  }
}
