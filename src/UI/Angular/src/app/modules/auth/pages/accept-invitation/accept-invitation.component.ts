import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AnchorComponent } from '../../../../shared/ui/anchor/anchor.component';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { InputComponent } from '../../../../shared/ui/input/input.component';
import {
  AlertNotification,
  FieldValidation,
  Regex,
  queryStringParams,
} from '../../../../shared/constants/constants';
import { authPaths } from '../../../../shared/constants/routes';
import { AlertService } from '../../../../shared/services/alert.service';
import { AuthService } from '../../services/auth.service';
import { HttpStatusCode } from '@angular/common/http';
import { IUserInviteDetails } from '../../models/user-invite-details.interface';

@Component({
  selector: 'app-accept-invitation',
  standalone: true,
  imports: [
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    InputComponent,
    ButtonComponent,
    AnchorComponent,
  ],
  templateUrl: './accept-invitation.component.html',
  styleUrl: './accept-invitation.component.css',
})
export class AcceptInvitationComponent implements OnInit {
  signinRoute = '/' + authPaths.signin;
  formAcceptInvitation!: FormGroup;
  isSubmitFormAcceptInvitation = false;
  userId = '';

  router = inject(Router);
  fb = inject(FormBuilder);
  alertService = inject(AlertService);
  authService = inject(AuthService);

  userInviteDto: IUserInviteDetails = {
    email: '',
    firstName: '',
    lastName: '',
    invitedBy: '',
  };

  constructor(private route: ActivatedRoute) {
    this.initForm();
  }

  ngOnInit(): void {
    this.userId =
      this.route.snapshot.queryParamMap.get(queryStringParams.UserId) ?? '';
    if (this.userId) {
      this.authService.getInviteDetails(this.userId).subscribe((res) => {
        if (res.success) {
          if (res.statusCode == HttpStatusCode.Conflict) {
            this.alertService.showMessage(
              'Invitation has already been accepted!',
              AlertNotification.type.error,
            );
            setTimeout(() => {
              this.router.navigate(['/' + authPaths.signin]);
            }, 1000);
          } else {
            this.userInviteDto = res.data;
          }
        }
      });
    }
  }

  initForm() {
    this.formAcceptInvitation = this.fb.group({
      password: [
        '',
        Validators.compose([
          Validators.required,
          Validators.minLength(FieldValidation.passwordMinLength),
          Validators.pattern(Regex.passwordValidationPattern),
        ]),
      ],
    });
  }

  submitFormAcceptInvitation() {
    this.isSubmitFormAcceptInvitation = true;
    if (this.formAcceptInvitation.valid) {
      const formValues = this.formAcceptInvitation.value;
      const acceptInvitationObj = {
        userId: this.userId,
        password: formValues.password,
      };
      this.authService.acceptInvitation(acceptInvitationObj).subscribe(
        (res: any) => {
          this.isSubmitFormAcceptInvitation = false;
          this.alertService.showMessage(res.data);
          this.formAcceptInvitation.reset();
          this.router.navigate(['/' + authPaths.signin]);
        },
        (error) => {},
      );
    }
  }
}
