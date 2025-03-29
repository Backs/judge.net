import React, {useEffect, useState} from "react";
import {Link, useParams} from "react-router-dom";
import {judgeApi} from "../../api/JudgeApi.ts";
import {handleError} from "../../helpers/handleError.ts";
import {ColumnType} from "antd/lib/table";
import {Flex, Table} from "antd";
import Title from "antd/lib/typography/Title";
import {ContestResult} from "../../api/Api.ts";
import {convertUserResult, ContestResultRow} from "../../helpers/contestResultHelper.tsx";
import {ContestDuration} from "./ContestDuration.tsx";

export const ContestStandings: React.FC = () => {
    const {contestId} = useParams();
    const [isLoading, setLoading] = useState(true);
    const [columns, setColumns] = useState<ColumnType<ContestResultRow>[]>([]);
    const [results, setResults] = useState<ContestResultRow[]>([]);
    const [contest, setContest] = useState<ContestResult>();

    const api = judgeApi();

    useEffect(() => {
        const fetchData = async () => {
            const response = await api.api.contestsResultsDetail(Number(contestId));

            setContest(response.data);

            const columns: ColumnType<ContestResultRow>[] = [
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
            } as ColumnType<ContestResultRow>));

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

            const userResults = response.data.users.map(t => convertUserResult(response.data.rules, t));

            setResults(userResults);

            setLoading(false);
        }

        fetchData().catch(e => handleError(e));
    }, [contestId]);

    return (<Flex gap="small" vertical>
        <Title style={{textAlign: 'center'}}>{contest?.name}</Title>
        <div style={{textAlign: 'center'}}>
            Start date: {contest?.startDate}
        </div>
        <div style={{textAlign: 'center'}}>
            <ContestDuration endDate={contest?.endDate} duration={contest?.duration}/>
        </div>
        <div style={{textAlign: 'center'}}>
            <Link to="./..">Problems</Link>
        </div>
        <Table
            dataSource={results}
            columns={columns}
            pagination={false}
            loading={isLoading}
        />
    </Flex>)
};