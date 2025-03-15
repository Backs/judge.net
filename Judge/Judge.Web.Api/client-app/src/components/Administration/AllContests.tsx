import React, {useEffect, useState} from "react";
import {Link, useNavigate, useSearchParams} from "react-router-dom";
import {judgeApi} from "../../api/JudgeApi.ts";
import {handleError} from "../../helpers/handleError.ts";
import {getColor, getStatusTest} from "../../helpers/contestStatusHelper.ts";
import {Pagination, Table, Tag} from "antd";
import {ColumnType} from "antd/lib/table";
import {UserState} from "../../userSlice.ts";
import {useSelector} from "react-redux";
import {ContestInfo} from "../../api/Api.ts";
import {MinusCircleOutlined} from "@ant-design/icons";

export const AllContests: React.FC = () => {
    const navigate = useNavigate();
    const [contestList, setContestList] = useState<ContestInfo[]>([]);
    const [searchParams, setSearchParams] = useSearchParams({page: "1", size: "10"});
    const [isLoading, setLoading] = useState(true);
    const [total, setTotal] = useState(0);

    const {user}: UserState = useSelector((state: any) => state.user)
    const isAdmin = user?.roles.includes("admin") || false;

    if (!isAdmin) {
        navigate("/login");
    }

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
            render: (_, record) => <Link to={`/contests/${record.id.toString()}/edit`}>{record.name} {!record.isOpened && <MinusCircleOutlined />}</Link>
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
            <Link to="/contests/new">Create new</Link>
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