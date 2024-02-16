import { DataTable, DataTableSortStatus } from "mantine-datatable";
import { useEffect, useState, useMemo } from "react";
import Tippy from "@tippyjs/react";
import "tippy.js/dist/tippy.css";
import { useDispatch } from "react-redux";
import { setPageTitle } from "../../../store/themeConfigSlice";
import axiosInstance from "../../../utils/api.service";
import { APIs } from "../../../utils/common/api-paths";
import { PaginationFilter } from "../../../utils/types/pagination-filter.interface";
import filterService from "../../../utils/filter.service";
import { dataTableProps } from "../../../utils/common/constants";
import { pageTitle } from "../../../utils/common/route-paths";
import messageService from "../../../utils/message.service";
import DeleteUserModal from "../../../components/Shared/delete-modal";
import ManageUserModal from "../components/manage-user";
import { ActionIcon, TextInput, Select } from "@mantine/core";
import { useDebouncedValue } from "@mantine/hooks";
import SearchIcon from "../../../components/Shared/Icons/search-icon";
import CloseIcon from "../../../components/Shared/Icons/close-icon";

const Users = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(setPageTitle(pageTitle.users));
  });

  const PAGE_SIZES = dataTableProps.PAGE_SIZES;
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(PAGE_SIZES[0]);
  const [recordsData, setRecordsData] = useState({ data: [], all_counts: 0 });
  const [params, setParams] = useState(filterService.defaultFilter());
  const [sortStatus, setSortStatus] = useState<DataTableSortStatus>({
    columnAccessor: "fullName",
    direction: "asc",
  });
  const [isdeleteUserModal, setIsDeleteUserModal] = useState<any>(false);
  const [deletedUserId, setdeletedUserId] = useState<string>("");
  const [isManageUserModal, setIsManageUserModal] = useState<any>(false);
  const [managedUserId, setManagedUserId] = useState<string>("");
  const [fullNameFilter, setFullNameFilter] = useState("");
  const [emailFilter, setEmailFilter] = useState("");
  const [debouncedFullNameFilter] = useDebouncedValue(fullNameFilter, 500);
  const [debouncedEmailFilter] = useDebouncedValue(emailFilter, 500);
  const [selectedStatus, setSelectedStatus] = useState("");

  const status: any = useMemo(() => {
    const status = ["Active", "Invited"];
    return [...status];
  }, []);

  useEffect(() => {
    const filterarray: any[] = [];
    if (debouncedFullNameFilter) {
      filterarray.push({
        Field: "fullName",
        Value: debouncedFullNameFilter,
        Operator: "contain",
      });
    }
    if (debouncedEmailFilter) {
      filterarray.push({
        Field: "email",
        Value: debouncedEmailFilter,
        Operator: "contain",
      });
    }
    if (selectedStatus) {
      filterarray.push({
        Field: "status",
        Value: selectedStatus,
        Operator: "contain",
      });
    }
    setParams({
      PageNumber: page,
      PageSize: pageSize,
      OrderBy: [sortStatus.columnAccessor + " " + sortStatus.direction],
      AdvancedFilter:
        filterarray.length == 0
          ? null
          : {
              Logic: "and",
              Filters: filterarray,
            },
    });
  }, [
    sortStatus,
    page,
    pageSize,
    debouncedFullNameFilter,
    debouncedEmailFilter,
    selectedStatus,
  ]);

  useEffect(() => {
    bindUsers(params);
  }, [params]);

  const bindUsers = async (params: PaginationFilter) => {
    const response = await axiosInstance.post(APIs.searchUserApi, params);
    if (response.data) {
      setRecordsData({
        data: response.data.data,
        all_counts: response.data.totalCount,
      });
    }
  };

  const deleteUserConfirm = (id: string) => {
    setdeletedUserId(id);
    setIsDeleteUserModal(true);
  };

  const deleteUser = async () => {
    const response = await axiosInstance.delete(
      APIs.deleteUserApi + deletedUserId
    );
    if (response.data) {
      messageService.showMessage(response.data.data);
      bindUsers(params);
      setIsDeleteUserModal(false);
    }
  };

  const manageUserConfirm = (id: string) => {
    setManagedUserId(id);
    setTimeout(() => {
      setIsManageUserModal(true);
    }, 100);
  };

  const onSaveManageUser = async () => {
    setManagedUserId("");
    bindUsers(params);
    setIsManageUserModal(false);
  };

  const onCloseManageUser = async () => {
    setManagedUserId("");
    setIsManageUserModal(false);
  };

  const getBadgeColor = (status: string) => {
    if (!status) return "";
    switch (status) {
      case "Invited":
        return "badge-outline-info";
      case "Active":
        return "badge-outline-success";
      case "Inactive":
        return "badge-outline-danger";
      default:
        return "badge-outline-info";
    }
  };

  return (
    <div className="panel">
      <div className="flex md:items-center md:flex-row flex-col mb-5 gap-5">
        <h5 className="font-semibold text-lg dark:text-white-light">Users</h5>
        <div className="ltr:ml-auto rtl:mr-auto">
          <button
            type="button"
            className="btn btn-outline-primary btn-sm"
            onClick={() => setIsManageUserModal(true)}
          >
            Invite
          </button>
        </div>
      </div>
      <div className="datatables">
        <DataTable
          highlightOnHover
          className="whitespace-nowrap table-hover"
          records={recordsData.data}
          columns={[
            {
              accessor: "fullName",
              title: "Full Name",
              filter: (
                <TextInput
                  label="Full Name"
                  description="Show users whose names include the specified text"
                  placeholder="Search full name..."
                  leftSection={<SearchIcon />}
                  rightSection={
                    <ActionIcon
                      size="sm"
                      variant="transparent"
                      c="dimmed"
                      onClick={() => setFullNameFilter("")}
                    >
                      <CloseIcon size={24} />
                    </ActionIcon>
                  }
                  value={fullNameFilter}
                  onChange={(e) => setFullNameFilter(e.currentTarget.value)}
                />
              ),
              filtering: fullNameFilter !== "",
              sortable: true,
            },
            {
              accessor: "email",
              title: "Email",
              filter: (
                <TextInput
                  label="Email"
                  description="Show users whose email include the specified text"
                  placeholder="Search email..."
                  leftSection={<SearchIcon />}
                  rightSection={
                    <ActionIcon
                      size="sm"
                      variant="transparent"
                      c="dimmed"
                      onClick={() => setEmailFilter("")}
                    >
                      <CloseIcon size={24} />
                    </ActionIcon>
                  }
                  value={emailFilter}
                  onChange={(e) => setEmailFilter(e.currentTarget.value)}
                />
              ),
              filtering: emailFilter !== "",
              sortable: true,
            },
            {
              accessor: "status",
              title: "Status",
              sortable: true,
              render: ({ status }) => (
                <span className={`badge ${getBadgeColor(`${status}`)} `}>
                  {`${status}`}
                </span>
              ),
              filter: (
                <Select
                  label="Status"
                  description="Show all users who has the selected status"
                  data={status}
                  value={selectedStatus}
                  placeholder="Search status"
                  onChange={(e) => setSelectedStatus(e ?? "")}
                  leftSection={<SearchIcon />}
                  clearable
                  searchable
                />
              ),
              filtering: selectedStatus.length > 0,
            },
            { accessor: "role", title: "Role", sortable: true },
            {
              accessor: "action",
              title: "Action",
              titleClassName: "!text-center",
              render: ({ id }) => (
                <div className="flex items-center w-max mx-auto gap-2">
                  <Tippy content="Edit">
                    <button
                      type="button"
                      onClick={() => manageUserConfirm(`${id}`)}
                    >
                      <svg
                        width="24"
                        height="24"
                        viewBox="0 0 24 24"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                        className="w-5 h-5 text-success"
                      >
                        <path
                          d="M15.2869 3.15178L14.3601 4.07866L5.83882 12.5999L5.83881 12.5999C5.26166 13.1771 4.97308 13.4656 4.7249 13.7838C4.43213 14.1592 4.18114 14.5653 3.97634 14.995C3.80273 15.3593 3.67368 15.7465 3.41556 16.5208L2.32181 19.8021L2.05445 20.6042C1.92743 20.9852 2.0266 21.4053 2.31063 21.6894C2.59466 21.9734 3.01478 22.0726 3.39584 21.9456L4.19792 21.6782L7.47918 20.5844L7.47919 20.5844C8.25353 20.3263 8.6407 20.1973 9.00498 20.0237C9.43469 19.8189 9.84082 19.5679 10.2162 19.2751C10.5344 19.0269 10.8229 18.7383 11.4001 18.1612L11.4001 18.1612L19.9213 9.63993L20.8482 8.71306C22.3839 7.17735 22.3839 4.68748 20.8482 3.15178C19.3125 1.61607 16.8226 1.61607 15.2869 3.15178Z"
                          stroke="currentColor"
                          strokeWidth="1.5"
                        />
                        <path
                          opacity="0.5"
                          d="M14.36 4.07812C14.36 4.07812 14.4759 6.04774 16.2138 7.78564C17.9517 9.52354 19.9213 9.6394 19.9213 9.6394M4.19789 21.6777L2.32178 19.8015"
                          stroke="currentColor"
                          strokeWidth="1.5"
                        />
                      </svg>
                    </button>
                  </Tippy>
                  <Tippy content="Delete">
                    <button
                      type="button"
                      onClick={() => deleteUserConfirm(`${id}`)}
                    >
                      <svg
                        className="text-danger"
                        width="20"
                        height="20"
                        viewBox="0 0 24 24"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                      >
                        <path
                          opacity="0.5"
                          d="M9.17065 4C9.58249 2.83481 10.6937 2 11.9999 2C13.3062 2 14.4174 2.83481 14.8292 4"
                          stroke="currentColor"
                          strokeWidth="1.5"
                          strokeLinecap="round"
                        />
                        <path
                          d="M20.5001 6H3.5"
                          stroke="currentColor"
                          strokeWidth="1.5"
                          strokeLinecap="round"
                        />
                        <path
                          d="M18.8334 8.5L18.3735 15.3991C18.1965 18.054 18.108 19.3815 17.243 20.1907C16.378 21 15.0476 21 12.3868 21H11.6134C8.9526 21 7.6222 21 6.75719 20.1907C5.89218 19.3815 5.80368 18.054 5.62669 15.3991L5.16675 8.5"
                          stroke="currentColor"
                          strokeWidth="1.5"
                          strokeLinecap="round"
                        />
                        <path
                          opacity="0.5"
                          d="M9.5 11L10 16"
                          stroke="currentColor"
                          strokeWidth="1.5"
                          strokeLinecap="round"
                        />
                        <path
                          opacity="0.5"
                          d="M14.5 11L14 16"
                          stroke="currentColor"
                          strokeWidth="1.5"
                          strokeLinecap="round"
                        />
                      </svg>
                    </button>
                  </Tippy>
                </div>
              ),
            },
          ]}
          totalRecords={recordsData.all_counts}
          recordsPerPage={pageSize}
          page={page}
          onPageChange={(p) => setPage(p)}
          recordsPerPageOptions={PAGE_SIZES}
          onRecordsPerPageChange={(newPageSize) => {
            setPageSize(newPageSize);
            setPage(1);
          }}
          sortStatus={sortStatus}
          onSortStatusChange={setSortStatus}
          minHeight={200}
          paginationText={({ from, to, totalRecords }) =>
            `Showing  ${from} to ${to} of ${totalRecords} entries`
          }
        />
      </div>
      <DeleteUserModal
        title="Delete User"
        message="Are you sure you want to delete this user?"
        isOpen={isdeleteUserModal}
        onClose={() => setIsDeleteUserModal(false)}
        onDelete={deleteUser}
      />
      <ManageUserModal
        manageUserId={managedUserId}
        isOpen={isManageUserModal}
        onClose={onCloseManageUser}
        onSave={onSaveManageUser}
      />
    </div>
  );
};

export default Users;
