import {ContestProblemResult, ContestRules, ContestUserResult} from "../api/Api.ts";
import {Typography} from 'antd';
import {BaseType} from "antd/es/typography/Base";

const {Text} = Typography;

export interface ContestResultRow {
    position: number,
    userName: string,
    solvedCount: number,
    points: number,

    [key: string]: any
}

function getAttempts(taskResult: ContestProblemResult) {
    if (taskResult.solved && taskResult.attempts === 1) {
        return "+";
    } else if (taskResult.solved) {
        return `+${taskResult.attempts - 1}`;
    } else {
        return `-${taskResult.attempts}`
    }
}

const formatAcmTaskResult = (taskResult: ContestProblemResult) => {
    const attempts = getAttempts(taskResult);

    const type: BaseType = taskResult.solved ? "success" : "danger";

    if (taskResult.time) {
        const time = taskResult.time;

        return (<Text type={type}>{attempts} <br/>{time} </Text>);
    }

    return (<Text type={type}>{attempts}</Text>);
}

const formatPointsTaskResult = (taskResult: ContestProblemResult) => {
    const attempts = getAttempts(taskResult);

    const type: BaseType = taskResult.solved ? "success" : "danger";

    if (taskResult.solved) {
        const points = taskResult.points;

        return (<Text type={type}>{attempts} <br/>{points} </Text>);
    }

    return (<Text type={type}>{attempts}</Text>);
}

export const formatTaskResult = (rules: ContestRules, taskResult: ContestProblemResult) => {
    switch (rules) {
        case ContestRules.Acm:
            return formatAcmTaskResult(taskResult);
        case ContestRules.Points:
        case ContestRules.Dynamic:
            return formatPointsTaskResult(taskResult);
        case ContestRules.CheckPoint:
            return formatAcmTaskResult(taskResult);
    }
}
export const convertUserResult = (rules: ContestRules, userResult: ContestUserResult) => {
    const row: ContestResultRow =
        {
            position: userResult.position,
            userName: userResult.userName,
            solvedCount: userResult.solvedCount,
            points: userResult.points
        };

    for (let label in userResult.tasks) {
        const task = userResult.tasks[label];

        row[label] = formatTaskResult(rules, task);
    }
    return row;
}