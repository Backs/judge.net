import {ContestStatus} from "../api/Api.ts";

export const getStatusTest = (status: ContestStatus) => {
    switch (status) {
        case ContestStatus.Planned:
            return "Planned";
        case ContestStatus.Running:
            return "Running...";
        case ContestStatus.Completed:
            return "Completed";
    }
}

export const getColor = (status: ContestStatus) => {
    switch (status) {
        case ContestStatus.Planned:
            return "processing";
        case ContestStatus.Running:
            return "success";
        case ContestStatus.Completed:
            return "warning";

    }
}