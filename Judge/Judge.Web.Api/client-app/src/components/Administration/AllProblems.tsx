import React, {useState, useEffect} from "react";
import {Pagination, Table} from "antd";
import {Link, useNavigate, useSearchParams} from "react-router-dom";
import {handleError} from "../../helpers/handleError.ts";
import {judgeApi} from "../../api/JudgeApi.ts";
import {MinusCircleOutlined} from '@ant-design/icons';
import {ColumnType} from "antd/lib/table";
import {UserState} from "../../userSlice.ts";
import {useSelector} from "react-redux";
import {AllProblemInfo} from "../../api/Api.ts";

export const AllProblems: React.FC = () => {
    const navigate = useNavigate();
    const [problemsList, setProblemsList] = useState<AllProblemInfo[]>([]);
    const [searchParams, setSearchParams] = useSearchParams({page: "1", size: "10"});
    const [total, setTotal] = useState(0);
    const [isLoading, setLoading] = useState(true);
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
            const response = await api.api.adminProblemsList({skip: skip, take: size});
            const items = response.data.items;

            setProblemsList(items);
            setTotal(response.data.totalCount);

            setLoading(false);
        };

        fetchData().catch(e => handleError(e));
    }, [searchParams]);

    const columns: ColumnType<AllProblemInfo>[] = [
        {
            title: 'Id',
            dataIndex: 'id',
            key: 'id',
        },
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'name',
            render: (_, problem) =>
                <Link to={`/problems/${problem.id}/edit`}>{problem.name} {!problem.isOpened &&
                    <MinusCircleOutlined/>}</Link>
        }
    ];

    return (
        <div>
            <Link to="/problems/new">Create new</Link>
            <br/>
            <br/>
            <Table dataSource={problemsList} columns={columns} pagination={false} loading={isLoading}/>
            <Pagination defaultCurrent={Number(searchParams.get("page"))} total={total}
                        defaultPageSize={Number(searchParams.get("size"))} onChange={(pageNumber, pageSize) => {
                setSearchParams({page: pageNumber.toString(), size: pageSize.toString()})
            }}/>
        </div>
    );
};