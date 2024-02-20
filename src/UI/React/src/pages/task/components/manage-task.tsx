import { Fragment, useEffect, useState } from "react";
import { Transition, Dialog } from "@headlessui/react";
import { Field, Form, Formik } from "formik";
import * as Yup from "yup";
import messageService from "../../../utils/message.service";
import { APIs } from "../../../utils/common/api-paths";
import axiosInstance from "../../../utils/api.service";
import { useDispatch } from "react-redux";
import commonService from "../../../utils/common.service";
import { ISelectItems } from "../../../utils/types";
import DatePicker from "../../../components/Shared/DatePicker";
import TextEditor from "../../../components/Shared/TextEditor";
import { FieldValidation } from "../../../utils/common/constants";

interface ManageTaskModalProps {
  manageTaskId: string;
  isOpen: boolean;
  onClose: () => void;
  onSave: () => void;
}

const ManageTaskModal: React.FC<ManageTaskModalProps> = ({
  manageTaskId,
  isOpen,
  onClose,
  onSave,
}) => {
  const dispatch = useDispatch();

  const [taskDetails, settaskDetails] = useState<{
    taskName: string;
    projectId: string;
    assignedTo: string;
    status: string;
    priority: string;
    deadline: string;
  }>({
    taskName: "",
    projectId: "",
    assignedTo: "",
    status: "",
    priority: "",
    deadline: "",
  });
  const [description, setDescription] = useState("");

  const [projectsOptions, setProjectsOptions] = useState<ISelectItems[]>([]);
  const [assignToUsersOptions, setAssignToUsersOptions] = useState<
    ISelectItems[]
  >([]);
  const [statusOptions, setStatusOptions] = useState<ISelectItems[]>([]);
  const [priorityOptions, setPriorityOptions] = useState<ISelectItems[]>([]);

  useEffect(() => {
    const bindProjectDdl = async () => {
      const response = await axiosInstance.get(APIs.getProjectListApi);

      if (response.data) {
        setProjectsOptions(
          response.data.data.map(
            ({ id, projectName }: { id: any; projectName: string }) => ({
              value: `${id}`,
              label: projectName,
            })
          )
        );
      }
    };
    const bindAssignToUserDdl = async () => {
      const response = await axiosInstance.get(APIs.getAssigneeListApi);

      if (response.data) {
        setAssignToUsersOptions(
          response.data.data.map(({ id, name }: { id: any; name: string }) => ({
            value: `${id}`,
            label: name,
          }))
        );
      }
    };
    const bindStatusDdl = async () => {
      const response = await axiosInstance.get(APIs.getTaskStatusListApi);

      if (response.data) {
        setStatusOptions(
          response.data.data.map(({ id, name }: { id: any; name: string }) => ({
            value: `${id}`,
            label: name,
          }))
        );
      }
    };
    const bindPriorityDdl = async () => {
      const response = await axiosInstance.get(APIs.getTaskPriorityListApi);

      if (response.data) {
        setPriorityOptions(
          response.data.data.map(({ id, name }: { id: any; name: string }) => ({
            value: `${id}`,
            label: name,
          }))
        );
      }
    };
    bindProjectDdl();
    bindAssignToUserDdl();
    bindStatusDdl();
    bindPriorityDdl();
  }, [dispatch]);

  useEffect(() => {
    const fetchtaskDetails = async () => {
      const response = await axiosInstance.get(APIs.getTaskApi + manageTaskId);
      if (response.data)
        settaskDetails({
          taskName: response.data.data.taskName,
          projectId: response.data.data.projectId,
          assignedTo: response.data.data.assignedTo,
          status: response.data.data.status,
          priority: response.data.data.priority,
          deadline:
            response.data.data.startDate + " to " + response.data.data.endDate,
        });
      setTimeout(() => {
        setDescription(response.data.data.description);
      }, 100);
    };

    if (manageTaskId) {
      fetchtaskDetails();
    }
  }, [manageTaskId]);

  const resetForm = () => {
    settaskDetails({
      taskName: "",
      projectId: "",
      assignedTo: "",
      status: "",
      priority: "",
      deadline: "",
    });
    setDescription("");
  };

  const resetAndClose = () => {
    if (manageTaskId) resetForm();
    onClose();
  };

  const submitForm = async (values: any) => {
    const dates = commonService.parseDateRange(values.deadline);
    const response = manageTaskId
      ? await axiosInstance.put(APIs.updateTaskApi + manageTaskId, {
          id: manageTaskId,
          taskName: values.taskName,
          projectId: values.projectId,
          assignedTo: values.assignedTo,
          status: values.status,
          priority: values.priority,
          startDate: dates.startDate,
          endDate: dates.endDate,
          description: description,
        })
      : await axiosInstance.post(APIs.createTaskApi, {
          taskName: values.taskName,
          projectId: values.projectId,
          assignedTo: values.assignedTo,
          status: values.status,
          priority: values.priority,
          startDate: dates.startDate,
          endDate: dates.endDate,
          description: description,
        });

    if (response.data) {
      messageService.showMessage(response.data.message);
      resetForm();
      onSave();
    }
  };

  const SubmittedForm = Yup.object().shape({
    taskName: Yup.string()
      .required("This can not be empty")
      .max(
        FieldValidation.taskNameMaxLength,
        `Task name must be at most ${FieldValidation.taskNameMaxLength} characters`
      ),
    projectId: Yup.string().required("This can not be empty"),
    assignedTo: Yup.string().required("This can not be empty"),
    status: Yup.string().required("This can not be empty"),
    priority: Yup.string().required("This can not be empty"),
    deadline: Yup.string().required("This can not be empty"),
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
                  {manageTaskId ? "Edit Task" : "Add Task"}
                </div>
                <div className="p-5">
                  <Formik
                    initialValues={{
                      taskName: taskDetails.taskName,
                      projectId: taskDetails.projectId,
                      assignedTo: taskDetails.assignedTo,
                      status: taskDetails.status,
                      priority: taskDetails.priority,
                      deadline: taskDetails.deadline,
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
                              ? errors.taskName
                                ? "has-error"
                                : ""
                              : ""
                          }
                        >
                          <label htmlFor="taskName">Task Name</label>
                          <Field
                            name="taskName"
                            type="text"
                            id="taskName"
                            placeholder="Enter Task Name"
                            className="form-input"
                          />
                          {submitCount ? (
                            errors.taskName ? (
                              <div className="text-danger mt-1">
                                {errors.taskName}
                              </div>
                            ) : (
                              ""
                            )
                          ) : (
                            ""
                          )}
                        </div>
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                          <div
                            className={
                              submitCount
                                ? errors.projectId
                                  ? "has-error"
                                  : ""
                                : ""
                            }
                          >
                            <label htmlFor="projectId">Project</label>
                            <Field
                              as="select"
                              name="projectId"
                              className="form-select"
                            >
                              <option value="">Select Project</option>
                              {projectsOptions.map((projectsOption) => {
                                return (
                                  <option
                                    key={projectsOption.value}
                                    value={projectsOption.value}
                                  >
                                    {projectsOption.label}
                                  </option>
                                );
                              })}
                            </Field>
                            {submitCount ? (
                              errors.projectId ? (
                                <div className=" text-danger mt-1">
                                  {errors.projectId}
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
                                ? errors.assignedTo
                                  ? "has-error"
                                  : ""
                                : ""
                            }
                          >
                            <label htmlFor="assignedTo">Assignee</label>
                            <Field
                              as="select"
                              name="assignedTo"
                              className="form-select"
                            >
                              <option value="">Select Assignee</option>
                              {assignToUsersOptions.map(
                                (assignToUsersOption) => {
                                  return (
                                    <option
                                      key={assignToUsersOption.value}
                                      value={assignToUsersOption.value}
                                    >
                                      {assignToUsersOption.label}
                                    </option>
                                  );
                                }
                              )}
                            </Field>
                            {submitCount ? (
                              errors.assignedTo ? (
                                <div className=" text-danger mt-1">
                                  {errors.assignedTo}
                                </div>
                              ) : (
                                ""
                              )
                            ) : (
                              ""
                            )}
                          </div>
                        </div>
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                          <div
                            className={
                              submitCount
                                ? errors.status
                                  ? "has-error"
                                  : ""
                                : ""
                            }
                          >
                            <label htmlFor="status">Status</label>
                            <Field
                              as="select"
                              name="status"
                              className="form-select"
                            >
                              <option value="">Select Status</option>
                              {statusOptions.map((statusOption) => {
                                return (
                                  <option
                                    key={statusOption.value}
                                    value={statusOption.value}
                                  >
                                    {statusOption.label}
                                  </option>
                                );
                              })}
                            </Field>
                            {submitCount ? (
                              errors.status ? (
                                <div className=" text-danger mt-1">
                                  {errors.status}
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
                                ? errors.priority
                                  ? "has-error"
                                  : ""
                                : ""
                            }
                          >
                            <label htmlFor="priority">Priority</label>
                            <Field
                              as="select"
                              name="priority"
                              className="form-select"
                            >
                              <option value="">Select Priority</option>
                              {priorityOptions.map((priorityOption) => {
                                return (
                                  <option
                                    key={priorityOption.value}
                                    value={priorityOption.value}
                                  >
                                    {priorityOption.label}
                                  </option>
                                );
                              })}
                            </Field>
                            {submitCount ? (
                              errors.priority ? (
                                <div className=" text-danger mt-1">
                                  {errors.priority}
                                </div>
                              ) : (
                                ""
                              )
                            ) : (
                              ""
                            )}
                          </div>
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
                            value={taskDetails.deadline}
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
                                (manageTaskId ||
                                  Object.keys(touched).length !== 0) &&
                                Object.keys(errors).length === 0
                              ) {
                                submitForm(values);
                              }
                            }}
                          >
                            {manageTaskId ? "Update" : "Create"}
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

export default ManageTaskModal;
