import React, {useState, useEffect} from "react";
import {Api} from "../../api/Judge.ts";
import {Pagination, Table} from "antd";
import {Link, useSearchParams} from "react-router-dom";
import {handleError} from "../../helpers/handleError.ts";

interface ProblemItem {
    id: number,
    name: any,
    solved: boolean
}

export const Problems: React.FC = () => {
    const [problemsList, setProblemsList] = useState<ProblemItem[]>([]);
    const [searchParams, setSearchParams] = useSearchParams();
    const [total, setTotal] = useState(0);
    const [isLoading, setLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            const api = new Api();
            const pageParam = searchParams.get("page") || 1;
            const sizeParam = searchParams.get("size") || 10;
            const skip = (Number(pageParam) - 1) * Number(sizeParam);
            const response = await api.api.problemsList({Skip: skip, Take: Number(sizeParam)});
            const items = response.data.items;

            const result: ProblemItem[] = items.map(p => ({
                id: p.id,
                name: <Link to={p.id.toString()}>{p.name}</Link>,
                solved: p.solved
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
            dataIndex: 'id',
            key: 'id',
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
            <Pagination defaultCurrent={Number(searchParams.get("page")) || 1} total={total}
                        defaultPageSize={Number(searchParams.get("size")) || 10} onChange={(pageNumber, pageSize) => {
                setSearchParams({page: pageNumber.toString(), size: pageSize.toString()})
            }}/>
        </div>
    );
};