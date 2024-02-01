import { Injectable } from '@angular/core';
import { appPaths } from '../../shared/constants/routes';

@Injectable({
  providedIn: 'root',
})
export class MenuService {
  getMenus(): Array<{ label: string; link: string }> {
    var menuItems: Array<{ label: string; link: string }> = [];
    menuItems.push({
      label: 'Users',
      link: '/' + appPaths.users,
    });

    menuItems.push({
      label: 'Projects',
      link: '/' + appPaths.projects,
    });

    menuItems.push({
      label: 'Tasks',
      link: '/' + appPaths.tasks,
    });
    return menuItems;
  }
}
