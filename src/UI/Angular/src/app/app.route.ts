import { Routes } from '@angular/router';
import { AppLayout } from './core/layouts/app-layout';
import { AuthLayout } from './core/layouts/auth-layout';
import { TodoListComponent } from './module/todo/pages/todo-list/todo-list.component';
import { SigninComponent } from './module/auth/pages/signin/signin.component';
import { SignupComponent } from './module/auth/pages/signup/signup.component';

export const routes: Routes = [
  {
    path: '',
    component: AppLayout,
    children: [
      {
        path: '',
        component: TodoListComponent,
        title: 'Home | Todo',
      },
    ],
  },
  {
    path: '',
    component: AuthLayout,
    children: [
      {
        path: 'signin',
        component: SigninComponent,
        title:
          'Signin | Todo',
      },
      {
        path: 'signup',
        component: SignupComponent,
        title:
          'Signup | Todo',
      },
    ],
  },
];
