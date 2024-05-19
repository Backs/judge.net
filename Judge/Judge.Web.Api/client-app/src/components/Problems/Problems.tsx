import React, {useState, useEffect} from "react";
import {Api, ProblemInfo} from "../../api/Api.ts";
import {Table} from "antd";

export const Problems: React.FC = () => {
    const [problemsList, setProblemsList] = useState<ProblemInfo[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            const api = new Api();
            const response = await api.api.problemsList();
            console.log(response);
            const items = response.data.items;

            if (response.data) {
                setProblemsList(items);
            }
        };

        fetchData();
    }, []);

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
            <Table dataSource={problemsList} columns={columns} pagination={false} />
        </div>
    );
};