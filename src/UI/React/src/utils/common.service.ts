class CommonService {
  parseDateRange(dateString: string) {
    const dates = dateString.split(" to ");

    const startDate = new Date(dates[0]);
    const endDate = dates[1] ? new Date(dates[1]) : startDate;

    const startDateUTC = new Date(
      Date.UTC(
        startDate.getFullYear(),
        startDate.getMonth(),
        startDate.getDate()
      )
    );
    const endDateUTC = new Date(
      Date.UTC(endDate.getFullYear(), endDate.getMonth(), endDate.getDate())
    );

    return {
      startDate: startDateUTC.toISOString().split("T")[0],
      endDate: endDateUTC.toISOString().split("T")[0],
    };
  }
}
export default new CommonService();
