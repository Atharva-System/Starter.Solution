import PerfectScrollbar from "react-perfect-scrollbar";
import { useDispatch, useSelector } from "react-redux";
import { NavLink, useLocation, useNavigate } from "react-router-dom";
import { toggleSidebar } from "../../store/themeConfigSlice";
import { IRootState } from "../../store";
import { useContext, useEffect, useState } from "react";
import menuService from "../../utils/menu.service";
import { IsFormDirtyContext } from "../Shared/Contexts";
import SaveOrDiscardModal from "../Shared/SaveOrDiscard";
import saveUnsavedChanges from "../../utils/save-or-discard.service";
import { APIs } from "../../utils/common/api-paths";
import { updateUserInfo } from "../../store/userInfoSlice";
import LocalStorageService from "../../utils/localstorage.service";
import messageService from "../../utils/message.service";
import { NotificationType } from "../../utils/common/constants";

const Sidebar = () => {
  const localStorageService = LocalStorageService.getService();
  const { param } = useContext(IsFormDirtyContext);
  const { resetFormDirty } = useContext(IsFormDirtyContext);
  const [isSaveOrDiscardModal, setIsSaveOrDiscardModal] =
    useState<boolean>(false);
  const [navigateTo, setNavigateTo] = useState<string>("/");
  const themeConfig = useSelector((state: IRootState) => state.themeConfig);
  const semidark = useSelector(
    (state: IRootState) => state.themeConfig.semidark
  );
  const location = useLocation();
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const menuItems = menuService.getMenus();

  useEffect(() => {
    const selector = document.querySelector(
      '.sidebar ul a[href="' + window.location.pathname + '"]'
    );
    if (selector) {
      selector.classList.add("active");
      const ul: any = selector.closest("ul.sub-menu");
      if (ul) {
        let ele: any =
          ul.closest("li.menu").querySelectorAll(".nav-link") || [];
        if (ele.length) {
          ele = ele[0];
          setTimeout(() => {
            ele.click();
          });
        }
      }
    }
  }, []);

  useEffect(() => {
    if (window.innerWidth < 1024 && themeConfig.sidebar) {
      dispatch(toggleSidebar());
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [location]);

  const onSaveChanges = async () => {
    if (param.isFormValid) {
      const response = await saveUnsavedChanges(param.apiCall);
      if (response) {
        stateUpdation();
      }
    } else {
      messageService.showMessage(
        "Failed to save changes! Invalid entries found in modified fields.",
        NotificationType.error
      );
    }
    setIsSaveOrDiscardModal(false);
    resetFormDirty();
    navigate(navigateTo);
  };

  const stateUpdation = () => {
    if (param.apiCall?.url.includes(APIs.updateProfile)) {
      const inputParam = param.apiCall.param;
      dispatch(
        updateUserInfo({
          fullName: inputParam.firstName + " " + inputParam.lastName,
          email: inputParam.email,
        })
      );
      localStorageService.updateStorageUserInfo(
        inputParam.firstName + " " + inputParam.lastName,
        inputParam.email
      );
    }
  };

  const onDiscardChanges = () => {
    setIsSaveOrDiscardModal(false);
    resetFormDirty();
    navigate(navigateTo);
  };

  const doNavigate = (path: string) => {
    if (param.isDirty) {
      setNavigateTo(path);
      setIsSaveOrDiscardModal(param.isDirty);
    } else {
      navigate(path);
    }
  };

  const isActive = (link: any) => {
    return location.pathname === link ? "active" : "";
  };

  return (
    <div className={semidark ? "dark" : ""}>
      <nav
        className={`sidebar fixed min-h-screen h-full top-0 bottom-0 w-[260px] shadow-[5px_0_25px_0_rgba(94,92,154,0.1)] z-50 transition-all duration-300 ${
          semidark ? "text-white-dark" : ""
        }`}
      >
        <div className="bg-white dark:bg-black h-full">
          <div className="flex justify-between items-center px-4 py-3">
            <NavLink to="/" className="main-logo flex items-center shrink-0">
              <img
                className="w-8 ml-[5px] flex-none"
                src="/assets/images/logo.svg"
                alt="logo"
              />
              <span className="text-2xl ltr:ml-1.5 rtl:mr-1.5 font-semibold align-middle lg:inline dark:text-white-light">
                STARTER
              </span>
            </NavLink>

            <button
              type="button"
              className="collapse-icon w-8 h-8 rounded-full flex items-center hover:bg-gray-500/10 dark:hover:bg-dark-light/10 dark:text-white-light transition duration-300 rtl:rotate-180"
              onClick={() => dispatch(toggleSidebar())}
            >
              <svg
                width="24"
                height="24"
                viewBox="0 0 24 24"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
                className="w-5 h-5 m-auto"
              >
                <path
                  d="M13 19L7 12L13 5"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
                <path
                  opacity="0.5"
                  d="M16.9998 19L10.9998 12L16.9998 5"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
              </svg>
            </button>
          </div>
          <PerfectScrollbar className="h-[calc(100vh-80px)] relative">
            <ul className="relative font-semibold space-y-0.5 p-4 py-0">
              <h2 className="py-3 px-7 flex items-center uppercase font-extrabold bg-white-light/30 dark:bg-dark dark:bg-opacity-[0.08] -mx-4 mb-1">
                <svg
                  className="w-4 h-5 flex-none hidden"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  fill="none"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                >
                  <line x1="5" y1="12" x2="19" y2="12"></line>
                </svg>
                <span>apps</span>
              </h2>
              <li className="nav-item">
                <ul>
                  {menuItems.map((menu) => {
                    return (
                      <li key={menu.label} className="nav-item">
                        <a
                          className={`group  cursor-pointer ${isActive(
                            menu.link
                          )}`}
                          onClick={() => doNavigate(menu.link)}
                        >
                          <div className="flex items-center">
                            {menu.label == "Users" && (
                              <svg
                                className="group-hover:!text-primary"
                                width="20"
                                height="20"
                                viewBox="0 0 24 24"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                              >
                                <circle
                                  opacity="0.5"
                                  cx="15"
                                  cy="6"
                                  r="3"
                                  fill="currentColor"
                                />
                                <ellipse
                                  opacity="0.5"
                                  cx="16"
                                  cy="17"
                                  rx="5"
                                  ry="3"
                                  fill="currentColor"
                                />
                                <circle
                                  cx="9.00098"
                                  cy="6"
                                  r="4"
                                  fill="currentColor"
                                />
                                <ellipse
                                  cx="9.00098"
                                  cy="17.001"
                                  rx="7"
                                  ry="4"
                                  fill="currentColor"
                                />
                              </svg>
                            )}
                            {menu.label == "Projects" && (
                              <svg
                                className="group-hover:!text-primary"
                                width="20"
                                height="20"
                                viewBox="0 0 24 24"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                              >
                                <path
                                  opacity="0.5"
                                  d="M21 15.9983V9.99826C21 7.16983 21 5.75562 20.1213 4.87694C19.3529 4.10856 18.175 4.01211 16 4H8C5.82497 4.01211 4.64706 4.10856 3.87868 4.87694C3 5.75562 3 7.16983 3 9.99826V15.9983C3 18.8267 3 20.2409 3.87868 21.1196C4.75736 21.9983 6.17157 21.9983 9 21.9983H15C17.8284 21.9983 19.2426 21.9983 20.1213 21.1196C21 20.2409 21 18.8267 21 15.9983Z"
                                  fill="currentColor"
                                />
                                <path
                                  d="M8 3.5C8 2.67157 8.67157 2 9.5 2H14.5C15.3284 2 16 2.67157 16 3.5V4.5C16 5.32843 15.3284 6 14.5 6H9.5C8.67157 6 8 5.32843 8 4.5V3.5Z"
                                  fill="currentColor"
                                />
                                <path
                                  fillRule="evenodd"
                                  clipRule="evenodd"
                                  d="M12 9.25C12.4142 9.25 12.75 9.58579 12.75 10V12.25L15 12.25C15.4142 12.25 15.75 12.5858 15.75 13C15.75 13.4142 15.4142 13.75 15 13.75L12.75 13.75L12.75 16C12.75 16.4142 12.4142 16.75 12 16.75C11.5858 16.75 11.25 16.4142 11.25 16L11.25 13.75H9C8.58579 13.75 8.25 13.4142 8.25 13C8.25 12.5858 8.58579 12.25 9 12.25L11.25 12.25L11.25 10C11.25 9.58579 11.5858 9.25 12 9.25Z"
                                  fill="currentColor"
                                />
                              </svg>
                            )}
                            {menu.label == "Tasks" && (
                              <svg
                                className="group-hover:!text-primary"
                                width="20"
                                height="20"
                                viewBox="0 0 24 24"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                              >
                                <path
                                  opacity="0.5"
                                  d="M3 10C3 6.22876 3 4.34315 4.17157 3.17157C5.34315 2 7.22876 2 11 2H13C16.7712 2 18.6569 2 19.8284 3.17157C21 4.34315 21 6.22876 21 10V14C21 17.7712 21 19.6569 19.8284 20.8284C18.6569 22 16.7712 22 13 22H11C7.22876 22 5.34315 22 4.17157 20.8284C3 19.6569 3 17.7712 3 14V10Z"
                                  fill="currentColor"
                                />
                                <path
                                  fillRule="evenodd"
                                  clipRule="evenodd"
                                  d="M12 5.25C12.4142 5.25 12.75 5.58579 12.75 6V7.25H14C14.4142 7.25 14.75 7.58579 14.75 8C14.75 8.41421 14.4142 8.75 14 8.75L12.75 8.75L12.75 10C12.75 10.4142 12.4142 10.75 12 10.75C11.5858 10.75 11.25 10.4142 11.25 10L11.25 8.75H9.99997C9.58575 8.75 9.24997 8.41421 9.24997 8C9.24997 7.58579 9.58575 7.25 9.99997 7.25H11.25L11.25 6C11.25 5.58579 11.5858 5.25 12 5.25ZM7.25 14C7.25 13.5858 7.58579 13.25 8 13.25H16C16.4142 13.25 16.75 13.5858 16.75 14C16.75 14.4142 16.4142 14.75 16 14.75H8C7.58579 14.75 7.25 14.4142 7.25 14ZM8.25 18C8.25 17.5858 8.58579 17.25 9 17.25H15C15.4142 17.25 15.75 17.5858 15.75 18C15.75 18.4142 15.4142 18.75 15 18.75H9C8.58579 18.75 8.25 18.4142 8.25 18Z"
                                  fill="currentColor"
                                />
                              </svg>
                            )}
                            <span className="ltr:pl-3 rtl:pr-3 text-black dark:text-[#506690] dark:group-hover:text-white-dark">
                              {menu.label}
                            </span>
                          </div>
                        </a>
                      </li>
                    );
                  })}
                </ul>
              </li>
            </ul>
          </PerfectScrollbar>
        </div>
      </nav>
      <SaveOrDiscardModal
        isOpen={isSaveOrDiscardModal}
        onClose={onDiscardChanges}
        onSave={onSaveChanges}
      ></SaveOrDiscardModal>
    </div>
  );
};

export default Sidebar;
