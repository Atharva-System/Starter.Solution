import { Fragment } from "react";
import { Transition, Dialog } from "@headlessui/react";
import { Field, Form, Formik } from "formik";
import * as Yup from "yup";
import messageService from "../../../utils/message.service";
import { APIs } from "../../../utils/common/api-paths";
import axiosInstance from "../../../utils/api.service";
import { Regex } from "../../../utils/common/constants";

interface ChangePasswordModalProps {
  isOpen: boolean;
  onClose: () => void;
}

const ChangePasswordModal: React.FC<ChangePasswordModalProps> = ({
  isOpen,
  onClose,
}) => {
  const submitForm = async (values: any) => {
    const response = await axiosInstance.post(APIs.changePasswordApi, values);

    if (response.data) {
      onClose();
      messageService.showMessage(response.data.message);
    }
  };

  const SubmittedForm = Yup.object().shape({
    currentPassword: Yup.string().required("This can not be empty"),
    newPassword: Yup.string()
      .required("This can not be empty")
      .min(6, "Confirm password must be at least 6 characters")
      .matches(
        Regex.passwordValidationPattern,
        "Password requirements: At least one uppercase letter (A-Z), one lowercase letter (a-z), and one non-alphanumeric character."
      ),
    confirmPassword: Yup.string()
      .required("This can not be empty")
      .oneOf([Yup.ref("newPassword"), ""], "Password must match"),
  });

  return (
    <Transition appear show={isOpen} as={Fragment}>
      <Dialog
        as="div"
        open={isOpen}
        onClose={() => (isOpen = true)}
        className="relative z-50"
      >
        <Transition.Child
          as={Fragment}
          enter="ease-out duration-300"
          enterFrom="opacity-0"
          enterTo="opacity-100"
          leave="ease-in duration-200"
          leaveFrom="opacity-100"
          leaveTo="opacity-0"
        >
          <div className="fixed inset-0 bg-[black]/60" />
        </Transition.Child>
        <div className="fixed inset-0 z-[999] px-4 overflow-y-auto">
          <div className="flex items-center justify-center min-h-screen">
            <Transition.Child
              as={Fragment}
              enter="ease-out duration-300"
              enterFrom="opacity-0 scale-95"
              enterTo="opacity-100 scale-100"
              leave="ease-in duration-200"
              leaveFrom="opacity-100 scale-100"
              leaveTo="opacity-0 scale-95"
            >
              <Dialog.Panel className="panel border-0 p-0 rounded-lg overflow-hidden w-full max-w-lg text-black dark:text-white-dark">
                <button
                  type="button"
                  onClick={onClose}
                  className="absolute top-4 ltr:right-4 rtl:left-4 text-gray-400 hover:text-gray-800 dark:hover:text-gray-600 outline-none"
                >
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="20"
                    height="20"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="1.5"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  >
                    <line x1="18" y1="6" x2="6" y2="18"></line>
                    <line x1="6" y1="6" x2="18" y2="18"></line>
                  </svg>
                </button>
                <div className="text-lg font-medium bg-[#fbfbfb] dark:bg-[#121c2c] ltr:pl-5 rtl:pr-5 py-3 ltr:pr-[50px] rtl:pl-[50px]">
                  Change Password
                </div>
                <div className="p-5">
                  <Formik
                    initialValues={{
                      currentPassword: "",
                      newPassword: "",
                      confirmPassword: "",
                    }}
                    validationSchema={SubmittedForm}
                    onSubmit={() => {}}
                  >
                    {({ errors, submitCount, touched, values }) => (
                      <Form className="space-y-5">
                        <span className="text-white-dark text-xs">
                          Password requirements: At least one uppercase letter
                          (A-Z), one lowercase letter (a-z), and one
                          non-alphanumeric character.
                        </span>
                        <div
                          className={
                            submitCount
                              ? errors.currentPassword
                                ? "has-error"
                                : ""
                              : ""
                          }
                        >
                          <label htmlFor="currentPassword">
                            Current Password{" "}
                          </label>
                          <Field
                            name="currentPassword"
                            type="password"
                            id="currentPassword"
                            placeholder="Enter Current Password"
                            className="form-input"
                          />

                          {submitCount ? (
                            errors.currentPassword ? (
                              <div className="text-danger mt-1">
                                {errors.currentPassword}
                              </div>
                            ) : (
                              ""
                            )
                          ) : (
                            ""
                          )}
                        </div>
                        <div
                          className={
                            submitCount
                              ? errors.newPassword
                                ? "has-error"
                                : ""
                              : ""
                          }
                        >
                          <label htmlFor="newPassword">New Password </label>
                          <Field
                            name="newPassword"
                            type="password"
                            id="newPassword"
                            placeholder="Enter New Password"
                            className="form-input"
                          />

                          {submitCount ? (
                            errors.newPassword ? (
                              <div className="text-danger mt-1">
                                {errors.newPassword}
                              </div>
                            ) : (
                              ""
                            )
                          ) : (
                            ""
                          )}
                        </div>
                        <div
                          className={
                            submitCount
                              ? errors.confirmPassword
                                ? "has-error"
                                : ""
                              : ""
                          }
                        >
                          <label htmlFor="confirmPassword">
                            Confirm Password{" "}
                          </label>
                          <Field
                            name="confirmPassword"
                            type="password"
                            id="confirmPassword"
                            placeholder="Confirm New Password"
                            className="form-input"
                          />

                          {submitCount ? (
                            errors.confirmPassword ? (
                              <div className="text-danger mt-1">
                                {errors.confirmPassword}
                              </div>
                            ) : (
                              ""
                            )
                          ) : (
                            ""
                          )}
                        </div>
                        <div className="flex justify-end items-center mt-8">
                          <button
                            type="button"
                            className="btn btn-outline-danger"
                            onClick={onClose}
                          >
                            Cancel
                          </button>
                          <button
                            type="submit"
                            className="btn btn-primary ltr:ml-4 rtl:mr-4"
                            onClick={() => {
                              if (
                                touched.currentPassword &&
                                !errors.currentPassword &&
                                touched.newPassword &&
                                !errors.newPassword &&
                                touched.confirmPassword &&
                                !errors.confirmPassword
                              ) {
                                submitForm(values);
                              }
                            }}
                          >
                            Save
                          </button>
                        </div>
                      </Form>
                    )}
                  </Formik>
                </div>
              </Dialog.Panel>
            </Transition.Child>
          </div>
        </div>
      </Dialog>
    </Transition>
  );
};

export default ChangePasswordModal;
