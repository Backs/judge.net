import React, {useEffect, useState} from "react";
import {Link, useSearchParams} from "react-router-dom";
import {judgeApi} from "../../api/JudgeApi.ts";
import {handleError} from "../../helpers/handleError.ts";
import {getColor, getStatusTest} from "../../helpers/contestStatusHelper.ts";
import {Pagination, Table, Tag} from "antd";
import {ColumnType} from "antd/lib/table";

interface ContestItem {
    key: number;
    name: any;
    startDate: string;
    duration: string;
    type: any;
    status: any;
}

export const AllContests: React.FC = () => {
    const [contestList, setContestList] = useState<ContestItem[]>([]);
    const [searchParams, setSearchParams] = useSearchParams({page: "1", size: "10"});
    const [isLoading, setLoading] = useState(true);
    const [total, setTotal] = useState(0);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            const api = judgeApi();
            const page = Number(searchParams.get("page"));
            const size = Number(searchParams.get("size"));
            const skip = (page - 1) * size;
            const response = await api.api.contestsList({Skip: skip, Take: size});
            const items = response.data.items;

            const result: ContestItem[] = items.map(p => ({
                key: p.id,
                name: <Link to={`/contests/${p.id.toString()}/edit`}>{p.name}</Link>,
                duration: p.duration,
                startDate: p.startDate,
                type: <Tag bordered={false} color="blue">{p.rules}</Tag>,
                status: <Tag bordered={false} color={getColor(p.status)}>{getStatusTest(p.status)}</Tag>
            }));

            setContestList(result);
            setTotal(response.data.totalCount);

            setLoading(false);
        };

        fetchData().catch(e => handleError(e));
    }, [searchParams]);

    const columns: ColumnType<ContestItem>[] = [
        {
            title: 'Id',
            dataIndex: 'key',
            key: 'key',
            align: 'right'
        },
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'key',
        },
        {
            title: 'Type',
            dataIndex: 'type',
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

    return (
        <div>
            <a href="/contests/new">Create new</a>
            <br />
            <br />
            <Table dataSource={contestList} columns={columns} pagination={false} loading={isLoading}/>
            <Pagination defaultCurrent={Number(searchParams.get("page"))} total={total}
                        defaultPageSize={Number(searchParams.get("size"))} onChange={(pageNumber, pageSize) => {
                setSearchParams({page: pageNumber.toString(), size: pageSize.toString()})
            }}/>
        </div>
    );
}