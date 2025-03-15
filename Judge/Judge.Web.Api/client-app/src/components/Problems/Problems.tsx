import React, {useState, useEffect} from "react";
import {Pagination, Table} from "antd";
import {Link, useSearchParams} from "react-router-dom";
import {handleError} from "../../helpers/handleError.ts";
import {judgeApi} from "../../api/JudgeApi.ts";
import {CheckOutlined, MinusCircleOutlined} from '@ant-design/icons';
import {ColumnType} from "antd/lib/table";
import {ProblemInfo} from "../../api/Api.ts";

export const Problems: React.FC = () => {
    const [problemsList, setProblemsList] = useState<ProblemInfo[]>([]);
    const [searchParams, setSearchParams] = useSearchParams({page: "1", size: "10"});
    const [total, setTotal] = useState(0);
    const [isLoading, setLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            const api = judgeApi();
            const page = Number(searchParams.get("page"));
            const size = Number(searchParams.get("size"));
            const skip = (page - 1) * size;
            const response = await api.api.problemsList({Skip: skip, Take: size});
            const items = response.data.items;

            setProblemsList(items);
            setTotal(response.data.totalCount);

            setLoading(false);
        };

        fetchData().catch(e => handleError(e));
    }, [searchParams]);

    const columns: ColumnType<ProblemInfo>[] = [
        {
            title: 'Id',
            dataIndex: 'id',
            key: 'id',
        },
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'name',
            render: (_, problem) => <Link to={problem.id.toString()} title="Hidden">{problem.name} {!problem.isOpened &&
                <MinusCircleOutlined/>}</Link>

        },
        {
            title: 'Solved',
            dataIndex: 'solved',
            key: 'solved',
            align: 'center',
            render: value => value && <CheckOutlined/>
        },
    ];

    return (
        <div>
            <Table dataSource={problemsList} columns={columns} pagination={false} loading={isLoading}/>
            <Pagination defaultCurrent={Number(searchParams.get("page"))} total={total}
                        defaultPageSize={Number(searchParams.get("size"))} onChange={(pageNumber, pageSize) => {
                setSearchParams({page: pageNumber.toString(), size: pageSize.toString()})
            }}/>
        </div>
    );
};