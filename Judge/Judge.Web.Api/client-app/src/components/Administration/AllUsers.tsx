import React, {useEffect, useState} from "react";
import {useNavigate, useSearchParams} from "react-router-dom";
import {judgeApi} from "../../api/JudgeApi.ts";
import {User} from "../../api/Api.ts";
import {Input, Pagination, Table} from "antd";
import {handleError} from "../../helpers/handleError.ts";
import {ColumnType} from "antd/lib/table";
import {UserState} from "../../userSlice.ts";
import {useSelector} from "react-redux";

export const AllUsers: React.FC = () => {
    const navigate = useNavigate();
    const [searchParams, setSearchParams] = useSearchParams({page: "1", size: "50", name: ""});
    const [isLoading, setLoading] = useState(true);
    const [users, setUsers] = useState<User[]>([]);
    const [total, setTotal] = useState(0);
    const api = judgeApi();

    const {user}: UserState = useSelector((state: any) => state.user)
    const isAdmin = user?.roles.includes("admin") || false;

    if (!isAdmin) {
        navigate("/login");
    }

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            const page = Number(searchParams.get("page"));
            const size = Number(searchParams.get("size"));
            const skip = (page - 1) * size;

            const name = searchParams.get("name") || "";
            const response = await api.api.usersList({Skip: skip, Take: size, Name: name});
            setUsers(response.data.items);
            setTotal(response.data.totalCount);


            setLoading(false);
        }
        fetchData().catch(e => handleError(e));
    }, [searchParams]);

    const columns: ColumnType<User>[] = [
        {
            title: 'Id',
            dataIndex: 'id',
            key: 'key',
        },
        {
            title: 'Login',
            dataIndex: 'login',
            key: 'login',
        },
        {
            title: 'Email',
            dataIndex: 'email',
            key: 'email'
        },
    ];

    return (
        <div>
            <Input
                placeholder="Search users"
                onChange={(e) => {
                    if (e.target.value.length > 1 || e.target.value.length === 0) {
                        setSearchParams({
                            page: "1",
                            size: "50",
                            name: e.target.value
                        });
                    }
                }}/>
            <Table dataSource={users} pagination={false} columns={columns} loading={isLoading}/>
            <Pagination defaultCurrent={Number(searchParams.get("page"))} total={total}
                        current={Number(searchParams.get("page"))}
                        defaultPageSize={Number(searchParams.get("size"))}
                        onChange={(pageNumber, pageSize) => {
                            setSearchParams({
                                page: pageNumber.toString(),
                                size: pageSize.toString(),
                                name: searchParams.get("name")!.toString()
                            })
                        }}/>
        </div>
    );
}