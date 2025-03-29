import React, {useEffect, useState} from "react";
import moment from "moment/moment";

interface ContestDurationProps {
    duration: string | undefined,
    endDate: string | undefined,
}

export const ContestDuration: React.FC<ContestDurationProps> = (props) => {
    const [remainingTime, setRemainingTime] = useState<string>();

    useEffect(() => {
        const time = () => {
            const currentDate = moment();
            const endDate = moment(props?.endDate);

            if (endDate > currentDate) {
                const remaining = endDate.diff(currentDate, "seconds");
                const days = Math.trunc(remaining / 86400);
                const hours = Math.trunc((remaining % 86400) / 3600);
                const minutes = Math.trunc((remaining % 3600) / 60);
                const seconds = Math.trunc(remaining % 60);

                const result = days + "." + String(hours).padStart(2, "0") + ":" + String(minutes).padStart(2, "0") + ":" + String(seconds).padStart(2, "0");
                setRemainingTime(result);
            } else {
                setRemainingTime(undefined);
            }
        };
        const intervalId = setInterval(time, 1000);

        return () => {
            clearInterval(intervalId);
        };
    }, [props]);

    return (<>
        {remainingTime && <span>Remaining time: {remainingTime}</span>}
        {!remainingTime && <span>Duration: {props?.duration}</span>}
    </>);
}