import React, {useState, useEffect} from "react";
import {Api, ProblemInfo} from "../../api/Api.ts";
import {Pagination, Table} from "antd";
import {useSearchParams} from "react-router-dom";

export const Problems: React.FC = () => {
    const [problemsList, setProblemsList] = useState<ProblemInfo[]>([]);
    const [searchParams, setSearchParams] = useSearchParams();
    const [total, setTotal] = useState(0);
    const [isLoading, setLoading] = useState(false);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            const api = new Api();
            const pageParam = searchParams.get("page") || 1;
            const sizeParam = searchParams.get("size") || 10;
            const skip = (Number(pageParam) - 1) * Number(sizeParam);
            const response = await api.api.problemsList({Skip: skip, Take: Number(sizeParam)});
            const items = response.data.items;

            if (response.data) {
                setProblemsList(items);
                setTotal(response.data.totalCount);
            }
            setLoading(false);
        };

        fetchData();
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