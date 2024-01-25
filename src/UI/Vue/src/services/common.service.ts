class CommonService {
    parseDateRange(dateString: string) {
        const dates = dateString.split(' to ');

        const startDate = new Date(dates[0]);
        const endDate = dates[1] ? new Date(dates[1]) : startDate;

        return {
            startDate: startDate.toISOString().split('T')[0],
            endDate: endDate.toISOString().split('T')[0],
        };
    }
}
export default new CommonService();