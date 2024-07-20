import React, {useEffect, useState} from "react";
import {Link, useParams} from "react-router-dom";
import {handleError} from "../../helpers/handleError.ts";
import {judgeApi} from "../../api/JudgeApi.ts";
import {Contest} from "../../api/Api.ts";
import {Flex, Spin, Table} from "antd";
import Title from "antd/lib/typography/Title";
import {CheckOutlined} from "@ant-design/icons";

interface ContestTaskItem {
    key: string,
    label: any,
    name: any,
    solved: any
}

export const ContestDetails: React.FC = () => {
    const {contestId} = useParams();
    const [isLoading, setLoading] = useState(true);
    const [contest, setContest] = useState<Contest>();
    const [tasks, setTasks] = useState<ContestTaskItem[]>([]);
    const api = judgeApi();

    useEffect(() => {
        const fetchData = async () => {
            const response = await api.api.contestsDetail(Number(contestId));

            const tasks = response.data.tasks.map(p => ({
                key: p.label,
                label: <Link to={p.label}>{p.label}</Link>,
                name: <Link to={p.label}>{p.name}</Link>,
                solved: p.solved && <CheckOutlined/>
            }));
            
            setTasks(tasks);
            setContest(response.data);

            document.title = `${response.data.name} - Judge.NET`;

            setLoading(false);
        }

        fetchData().catch(e => handleError(e));
    }, [contestId]);

    const columns = [
        {
            title: 'Label',
            dataIndex: 'label',
            key: 'key',
        },
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'key',
        },
        {
            title: 'Solved',
            dataIndex: 'solved',
            key: 'key',
        }
    ];

    return (isLoading ? <Spin size="large"/> : <>
            <Title style={{textAlign: 'center'}}>{contest?.name}</Title>
            <Flex gap="small" vertical>
                <div style={{textAlign: 'center'}}>
                    Start date: {contest?.startDate}
                </div>
                <div style={{textAlign: 'center'}}>
                    Duration: {contest?.duration}
                </div>
            </Flex>
            <Table dataSource={tasks} columns={columns} pagination={false} loading={isLoading}/>
        </>
    );
}