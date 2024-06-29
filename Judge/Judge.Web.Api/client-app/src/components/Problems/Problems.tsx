import React, {useState, useEffect} from "react";
import {Pagination, Table} from "antd";
import {Link, useSearchParams} from "react-router-dom";
import {handleError} from "../../helpers/handleError.ts";
import {judgeApi} from "../../api/JudgeApi.ts";
import {CheckOutlined} from '@ant-design/icons';

interface ProblemItem {
    key: number,
    name: any,
    solved: any
}

export const Problems: React.FC = () => {
    const [problemsList, setProblemsList] = useState<ProblemItem[]>([]);
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

            const result: ProblemItem[] = items.map(p => ({
                key: p.id,
                name: <Link to={p.id.toString()}>{p.name}</Link>,
                solved: p.solved && <CheckOutlined/>
            }));
            setProblemsList(result);
            setTotal(response.data.totalCount);

            setLoading(false);
        };

        fetchData().catch(e => handleError(e));
    }, [searchParams]);

    const columns = [
        {
            title: 'Id',
            dataIndex: 'key',
            key: 'key',
        },
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'name',
        },
        {
            title: 'Solved',
            dataIndex: 'solved',
            key: 'solved',
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