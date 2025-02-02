import React, {useEffect, useState} from "react";
import {Col, Row, Table, Tag} from "antd";
import {judgeApi} from "../../api/JudgeApi.ts";
import {Link} from "react-router-dom";
import {getColor, getStatusTest} from "../../helpers/contestStatusHelper.ts";
import {handleError} from "../../helpers/handleError.ts";
import {ColumnType} from "antd/lib/table";

interface ContestItem {
    key: number;
    name: any;
    startDate: string;
    duration: string;
    status: any;
}

export const UpcomingContests: React.FC = () => {
    const [contestList, setContestList] = useState<ContestItem[]>([]);
    const [isLoading, setLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            const api = judgeApi();
            const response = await api.api.contestsList({Skip: 0, Take: 10, UpcomingOnly: true});
            const items = response.data.items;

            const result: ContestItem[] = items.map(p => ({
                key: p.id,
                name: <Link to={`contests/${p.id}`}>{p.name}</Link>,
                duration: p.duration,
                startDate: p.startDate,
                status: <Tag bordered={false} color={getColor(p.status)}>{getStatusTest(p.status)}</Tag>
            }));

            setContestList(result);

            setLoading(false);
        };

        fetchData().catch(e => handleError(e));
    }, []);

    const columns: ColumnType<ContestItem>[] = [
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'key',
        },
        {
            title: 'Status',
            dataIndex: 'status',
            key: 'key',
        },
        {
            title: 'Start date',
            dataIndex: 'startDate',
            key: 'key',
            align: 'right'
        },
        {
            title: 'Duration',
            dataIndex: 'duration',
            key: 'key',
            align: 'right'
        }
    ];

    return (<div>
        <h3>Upcoming contests</h3>

        {contestList.length == 0 && <span>No contests.</span>}
        {contestList.length != 0 &&
            <Row>
                <Col span={12}>
                    <Table dataSource={contestList} columns={columns} pagination={false} loading={isLoading}/>
                </Col>
            </Row>
        }
    </div>)
}