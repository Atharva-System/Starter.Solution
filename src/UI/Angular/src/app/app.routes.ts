import { Routes } from '@angular/router';
import { AuthGuard } from './core/guard/auth.guard';
import { AppLayout } from './core/layouts/app-layout';
import { AuthLayout } from './core/layouts/auth-layout';
import { SigninComponent } from './module/auth/pages/signin/signin.component';
import { SignupComponent } from './module/auth/pages/signup/signup.component';
import { TodoListComponent } from './module/todo/pages/todo-list/todo-list.component';
import { appPaths, authPaths } from './shared/constants/routes';

export const routes: Routes =  [
    {
      path: '',
      component: AppLayout,
      children: [
        {
          path: '',
          component: TodoListComponent,
          title: 'Home | Todo',
        },
        {
          path: appPaths.todo,
          component: TodoListComponent,
          title: 'Home | Todo',
        },
      ],
      canActivate: [AuthGuard],
    },
    {
      path: '',
      component: AuthLayout,
      children: [
        {
          path: authPaths.signin,
          component: SigninComponent,
          title: 'Signin | Todo',
        },
        {
          path: authPaths.signup,
          component: SignupComponent,
          title: 'Signup | Todo',
        },
      ],
    },
  ];
  
