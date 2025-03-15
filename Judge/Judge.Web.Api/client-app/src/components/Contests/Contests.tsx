import React, {useEffect, useState} from "react";
import {Link, useSearchParams} from "react-router-dom";
import {judgeApi} from "../../api/JudgeApi.ts";
import {handleError} from "../../helpers/handleError.ts";
import {getColor, getStatusTest} from "../../helpers/contestStatusHelper.ts";
import {Pagination, Table, Tag} from "antd";
import {ColumnType} from "antd/lib/table";
import {ContestInfo} from "../../api/Api.ts";

export const Contests: React.FC = () => {
    const [contestList, setContestList] = useState<ContestInfo[]>([]);
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

            setContestList(items);
            setTotal(response.data.totalCount);

            setLoading(false);
        };

        fetchData().catch(e => handleError(e));
    }, [searchParams]);

    const columns: ColumnType<ContestInfo>[] = [
        {
            title: 'Id',
            dataIndex: 'id',
            key: 'id',
            align: 'right'
        },
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'name',
            render: (_, record) => <Link to={record.id.toString()}>{record.name}</Link>
        },
        {
            title: 'Status',
            dataIndex: 'status',
            key: 'status',
            render: (_, record) => <Tag bordered={false}
                                        color={getColor(record.status)}>{getStatusTest(record.status)}</Tag>
        },
        {
            title: 'Start date',
            dataIndex: 'startDate',
            key: 'startDate',
            align: 'right'
        },
        {
            title: 'Duration',
            dataIndex: 'duration',
            key: 'duration',
            align: 'right'
        }
    ];

    return (
        <div>
            <Table dataSource={contestList} columns={columns} pagination={false} loading={isLoading}/>
            <Pagination defaultCurrent={Number(searchParams.get("page"))} total={total}
                        defaultPageSize={Number(searchParams.get("size"))} onChange={(pageNumber, pageSize) => {
                setSearchParams({page: pageNumber.toString(), size: pageSize.toString()})
            }}/>
        </div>
    );
}