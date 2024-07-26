import React, {useEffect, useState} from "react";
import {Link, useParams} from "react-router-dom";
import {judgeApi} from "../../api/JudgeApi.ts";
import {handleError} from "../../helpers/handleError.ts";
import {ColumnType} from "antd/lib/table";
import {Flex, Table} from "antd";
import Title from "antd/lib/typography/Title";
import {ContestProblemResult, ContestResult, ContestUserResult} from "../../api/Api.ts";

interface Row {
    position: number,
    userName: string,
    solvedCount: number,
    points: number,

    [key: string]: any
}

export const ContestStandings: React.FC = () => {
    const {contestId} = useParams();
    const [isLoading, setLoading] = useState(true);
    const [columns, setColumns] = useState<ColumnType<Row>[]>([]);
    const [results, setResults] = useState<Row[]>([]);
    const [contest, setContest] = useState<ContestResult>();

    const api = judgeApi();

    useEffect(() => {
        const fetchData = async () => {
            const response = await api.api.contestsResultsDetail(Number(contestId));

            setContest(response.data);

            const columns: ColumnType<Row>[] = [
                {
                    title: 'Position',
                    dataIndex: 'position',
                    key: 'userName',
                    align: 'right'
                },
                {
                    title: 'User',
                    dataIndex: 'userName',
                    key: 'userName',
                },
            ];
            const problems = response.data.tasks.map(t => ({
                title: <Link to={`../contests/${contestId}/${t.label}`}>{t.label}</Link>,
                dataIndex: t.label,
                key: 'key',
                align: 'center'
            } as ColumnType<Row>));

            columns.push(...problems);
            columns.push(
                {
                    title: 'Solved',
                    dataIndex: 'solvedCount',
                    key: 'userName',
                },
                {
                    title: 'Total',
                    dataIndex: 'points',
                    key: 'userName',
                }
            );

            setColumns(columns);

            document.title = `${response.data.name} - Judge.NET`;

            const formatTaskResult = (taskResult: ContestProblemResult) => {
                if (taskResult.solved && taskResult.attempts === 1) {
                    return "+";
                } else if (taskResult.solved) {
                    return `+${taskResult.attempts}`;
                } else {
                    return `-${taskResult.attempts}`
                }

            }
            const convertUserResult = (userResult: ContestUserResult) => {
                const row: Row =
                    {
                        position: userResult.position,
                        userName: userResult.userName,
                        solvedCount: userResult.solvedCount,
                        points: userResult.points
                    };

                for (let label in userResult.tasks) {
                    const task = userResult.tasks[label];

                    row[label] = formatTaskResult(task);
                }
                return row;
            }

            const userResults = response.data.users.map(convertUserResult);

            setResults(userResults);

            setLoading(false);
        }

        fetchData().catch(e => handleError(e));
    }, [contestId]);

    return (<div>
        <Title style={{textAlign: 'center'}}>{contest?.name}</Title>
        <Flex gap="small" vertical>
            <div style={{textAlign: 'center'}}>
                Start date: {contest?.startDate}
            </div>
            <div style={{textAlign: 'center'}}>
                Duration: {contest?.duration}
            </div>
        </Flex>
        <Table dataSource={results} columns={columns} pagination={false} loading={isLoading}/>
    </div>)
};