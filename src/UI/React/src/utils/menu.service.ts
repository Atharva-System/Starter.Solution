import { appPaths } from "./common/route-paths";

class MenuService {
  getMenus(): Array<{ label: string; link: string }> {
    const menuItems: Array<{ label: string; link: string }> = [];
    menuItems.push({
      label: "Users",
      link: "/" + appPaths.users,
    });

    menuItems.push({
      label: "Projects",
      link: "/" + appPaths.projects,
    });

    menuItems.push({
      label: "Tasks",
      link: "/" + appPaths.tasks,
    });
    return menuItems;
  }
}
export default new MenuService();
