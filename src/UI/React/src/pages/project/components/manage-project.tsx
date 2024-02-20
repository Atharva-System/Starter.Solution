import { Fragment, useEffect, useState } from "react";
import { Transition, Dialog } from "@headlessui/react";
import { Field, Form, Formik } from "formik";
import * as Yup from "yup";
import messageService from "../../../utils/message.service";
import { APIs } from "../../../utils/common/api-paths";
import axiosInstance from "../../../utils/api.service";
import commonService from "../../../utils/common.service";
import DatePicker from "../../../components/Shared/DatePicker";
import TextEditor from "../../../components/Shared/TextEditor";
import { FieldValidation } from "../../../utils/common/constants";

interface ManageProjectModalProps {
  manageProjectId: string;
  isOpen: boolean;
  onClose: () => void;
  onSave: () => void;
}

const ManageProjectModal: React.FC<ManageProjectModalProps> = ({
  manageProjectId,
  isOpen,
  onClose,
  onSave,
}) => {
  const [projectDetails, setProjectDetails] = useState<{
    projectName: string;
    deadline: string;
    estimatedHours: string;
  }>({
    projectName: "",
    deadline: "",
    estimatedHours: "",
  });
  const [description, setDescription] = useState("");

  useEffect(() => {
    const fetchProjectDetails = async () => {
      const response = await axiosInstance.get(
        APIs.getProjectApi + manageProjectId
      );
      if (response.data)
        setProjectDetails({
          projectName: response.data.data.projectName,
          deadline:
            response.data.data.startDate + " to " + response.data.data.endDate,
          estimatedHours: response.data.data.estimatedHours,
        });

      setTimeout(() => {
        setDescription(response.data.data.description);
      }, 100);
    };

    if (manageProjectId) {
      fetchProjectDetails();
    }
  }, [manageProjectId]);

  const resetForm = () => {
    setProjectDetails({
      projectName: "",
      deadline: "",
      estimatedHours: "",
    });
    setDescription("");
  };

  const resetAndClose = () => {
    if (manageProjectId) resetForm();
    onClose();
  };

  const submitForm = async (values: any) => {
    const dates = commonService.parseDateRange(values.deadline);
    const response = manageProjectId
      ? await axiosInstance.put(APIs.updateProjectApi + manageProjectId, {
          id: manageProjectId,
          projectName: values.projectName,
          description: description,
          startDate: dates.startDate,
          endDate: dates.endDate,
          estimatedHours: values.estimatedHours,
        })
      : await axiosInstance.post(APIs.createProjectApi, {
          projectName: values.projectName,
          description: description,
          startDate: dates.startDate,
          endDate: dates.endDate,
          estimatedHours: values.estimatedHours,
        });

    if (response.data) {
      messageService.showMessage(response.data.data);
      resetForm();
      onSave();
    }
  };

  const SubmittedForm = Yup.object().shape({
    projectName: Yup.string()
      .required("This can not be empty")
      .max(
        FieldValidation.projectNameMaxLength,
        `Project name must be at most ${FieldValidation.projectNameMaxLength} characters`
      ),
    deadline: Yup.string().required("This can not be empty"),
    estimatedHours: Yup.string()
      .required("This can not be empty")
      .max(
        FieldValidation.estimatedHoursMaxLength,
        "Estimated hours cannot exceed " +
          FieldValidation.estimatedHoursMaxLength
      ),
    description: Yup.string().max(
      FieldValidation.descriptionMaxLength,
      `Description must be at most ${FieldValidation.descriptionMaxLength} characters`
    ),
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
                  onClick={resetAndClose}
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
                  {manageProjectId ? "Edit Project" : "Add Project"}
                </div>
                <div className="p-5">
                  <Formik
                    initialValues={{
                      projectName: projectDetails.projectName,
                      deadline: projectDetails.deadline,
                      estimatedHours: projectDetails.estimatedHours,
                      description: description,
                    }}
                    validationSchema={SubmittedForm}
                    onSubmit={() => {}}
                  >
                    {({
                      errors,
                      submitCount,
                      touched,
                      values,
                      setFieldValue,
                    }) => (
                      <Form className="space-y-5">
                        <div
                          className={
                            submitCount
                              ? errors.projectName
                                ? "has-error"
                                : ""
                              : ""
                          }
                        >
                          <label htmlFor="projectName">Project Name</label>
                          <Field
                            name="projectName"
                            type="text"
                            id="projectName"
                            placeholder="Enter Project Name"
                            className="form-input"
                          />
                          {submitCount ? (
                            errors.projectName ? (
                              <div className="text-danger mt-1">
                                {errors.projectName}
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
                              ? errors.description
                                ? "has-error"
                                : ""
                              : ""
                          }
                        >
                          <label htmlFor="description">Description</label>
                          <TextEditor
                            value={description}
                            onChange={(e) => {
                              setDescription(e);
                              setFieldValue("description", e);
                            }}
                            style={{ minHeight: "150px" }}
                          />
                          {submitCount ? (
                            errors.description ? (
                              <div className="text-danger mt-1">
                                {errors.description}
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
                              ? errors.deadline
                                ? "has-error"
                                : ""
                              : ""
                          }
                        >
                          <label htmlFor="deadline">Deadline</label>
                          <DatePicker
                            id="deadline"
                            name="deadline"
                            placeholder="Enter Deadline"
                            options={{
                              mode: "range",
                            }}
                            value={projectDetails.deadline}
                            onChange={(date: any, event: any) => {
                              setFieldValue("deadline", event);
                            }}
                          />
                          {submitCount ? (
                            errors.deadline ? (
                              <div className="text-danger mt-1">
                                {errors.deadline}
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
                              ? errors.estimatedHours
                                ? "has-error"
                                : ""
                              : ""
                          }
                        >
                          <label htmlFor="estimatedHours">
                            Estimated Hours
                          </label>
                          <Field
                            name="estimatedHours"
                            type="number"
                            id="estimatedHours"
                            placeholder="Enter Estimated Hours"
                            className="form-input"
                          />
                          {submitCount ? (
                            errors.estimatedHours ? (
                              <div className="text-danger mt-1">
                                {errors.estimatedHours}
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
                            onClick={resetAndClose}
                          >
                            Cancel
                          </button>
                          <button
                            type="submit"
                            className="btn btn-primary ltr:ml-4 rtl:mr-4"
                            onClick={() => {
                              if (
                                (manageProjectId ||
                                  Object.keys(touched).length !== 0) &&
                                Object.keys(errors).length === 0
                              ) {
                                submitForm(values);
                              }
                            }}
                          >
                            {manageProjectId ? "Update" : "Create"}
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

export default ManageProjectModal;
