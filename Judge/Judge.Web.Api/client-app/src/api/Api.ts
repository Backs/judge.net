/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

/** Contest */
export interface Contest {
  /**
   * Contest id
   * @format int32
   */
  id: number;
  /**
   * Contest name
   * @minLength 1
   */
  name: string;
  /**
   * Contest start date in UTC
   * @format date-time
   */
  startDate: string;
  /**
   * Contest duration
   * @example "00:00:00"
   */
  duration: string;
  /** Contest status */
  status: ContestStatus;
  tasks: ContestTask[];
}

/** Contest information */
export interface ContestInfo {
  /**
   * Contest id
   * @format int32
   */
  id: number;
  /**
   * Contest name
   * @minLength 1
   */
  name: string;
  /**
   * Contest start date in UTC
   * @format date-time
   */
  startDate: string;
  /**
   * Contest duration
   * @example "00:00:00"
   */
  duration: string;
  /** Contest status */
  status: ContestStatus;
}

/** Contest problem result */
export interface ContestProblemResult {
  /**
   * Task points
   * @format int32
   */
  points: number;
  /**
   * Total problem attempts
   * @format int32
   */
  attempts: number;
  /**
   * Problem solve time
   * @example "00:00:00"
   */
  time?: string | null;
  /** Problem solved by user */
  solved: boolean;
}

export interface ContestResult {
  /**
   * Contest id
   * @format int32
   */
  id: number;
  /**
   * Contest name
   * @minLength 1
   */
  name: string;
  /**
   * Contest start date in UTC
   * @format date-time
   */
  startDate: string;
  /**
   * Contest duration
   * @example "00:00:00"
   */
  duration: string;
  /** Contest status */
  status: ContestStatus;
  tasks: ContestTask[];
  /** Contest rules */
  rules: ContestRules;
  users: ContestUserResult[];
}

/** Contest rules */
export enum ContestRules {
  Acm = "Acm",
  Points = "Points",
  CheckPoint = "CheckPoint",
}

/** Contest status */
export enum ContestStatus {
  Planned = "Planned",
  Running = "Running",
  Completed = "Completed",
}

/** Contest task */
export interface ContestTask {
  /**
   * Unique task label
   * @minLength 1
   */
  label: string;
  /**
   * Task name
   * @minLength 1
   */
  name: string;
  /** Task solved by current user */
  solved: boolean;
}

/** Contest user results */
export interface ContestUserResult {
  /**
   * User position
   * @format int32
   */
  position: number;
  /**
   * User name
   * @minLength 1
   */
  userName: string;
  /**
   * User id
   * @format int64
   */
  userId: number;
  tasks: Record<string, ContestProblemResult>;
  /**
   * Total solved tasks
   * @format int32
   */
  solvedCount: number;
  /**
   * Total points
   * @format int32
   */
  points: number;
}

/** List of contests */
export interface ContestsInfoList {
  items: ContestInfo[];
  /**
   * Total contests count
   * @format int32
   */
  totalCount: number;
}

/** Create new user */
export interface CreateUser {
  /**
   * User email
   * @format email
   * @minLength 2
   */
  email: string;
  /**
   * User login
   * @minLength 2
   */
  login: string;
  /**
   * User password
   * @minLength 6
   */
  password: string;
}

/** Creates or edit contest */
export interface EditContest {
  /**
   * Contest id
   * @format int32
   */
  id?: number | null;
  /**
   * Contest name
   * @minLength 1
   * @maxLength 200
   */
  name: string;
  /**
   * Contest start time
   * @format date-time
   */
  startTime: string;
  /**
   * Contest finish time
   * @format date-time
   */
  finishTime: string;
  /**
   * Contest check point
   * @format date-time
   */
  checkPointTime?: string | null;
  /** Contest rules */
  rules: ContestRules;
  /** Contest problems */
  problems?: EditContestProblem[] | null;
  /** If true, only one language can be used for every task */
  oneLanguagePerTask?: boolean;
  /** If contest is shown for participants */
  isOpened?: boolean;
}

/** Contest problem */
export interface EditContestProblem {
  /**
   * Problem id
   * @format int32
   */
  problemId: number;
  /**
   * Problem label in contest
   * @minLength 1
   */
  label: string;
}

/** Creates or edit problem */
export interface EditProblem {
  /**
   * Problem id
   * @format int64
   */
  id?: number | null;
  /**
   * Problem name
   * @minLength 1
   */
  name: string;
  /**
   * Memory limits in bytes
   * @format int32
   */
  memoryLimitBytes: number;
  /**
   * Time limits in milliseconds
   * @format int32
   */
  timeLimitMilliseconds: number;
  /**
   * Problem statement in markdown
   * @minLength 1
   */
  statement: string;
  /**
   * Folder path with tests
   * @minLength 1
   */
  testsFolder: string;
  /** Is problem opened in archive */
  idOpened: boolean;
}

/** Login information */
export interface Login {
  /**
   * User email
   * @minLength 1
   */
  email: string;
  /**
   * User password
   * @minLength 1
   */
  password: string;
}

/** Login result */
export interface LoginResult {
  /**
   * Authentication token
   * @minLength 1
   */
  token: string;
}

export interface Problem {
  /**
   * Problem id
   * @format int64
   */
  id: number;
  /**
   * Problem name
   * @minLength 1
   */
  name: string;
  /**
   * Memory limit in bytes
   * @format int64
   */
  memoryLimitBytes: number;
  /**
   * Time limit in milliseconds
   * @format int64
   */
  timeLimitMilliseconds: number;
  /**
   * Problem statement in markdown
   * @minLength 1
   */
  statement: string;
}

export interface ProblemDetails {
  type?: string | null;
  title?: string | null;
  /** @format int32 */
  status?: number | null;
  detail?: string | null;
  instance?: string | null;
  [key: string]: any;
}

/** Problem info */
export interface ProblemInfo {
  /**
   * Problem id
   * @format int64
   */
  id: number;
  /**
   * Problem name
   * @minLength 1
   */
  name: string;
  /** Is problem solve by current user */
  solved: boolean;
}

/** Problem list */
export interface ProblemsList {
  /** Problems info */
  items: ProblemInfo[];
  /**
   * Total count of problems
   * @format int32
   */
  totalCount: number;
}

/** Contest information */
export interface SubmitResultContestInfo {
  /**
   * Contest id
   * @format int32
   */
  contestId: number;
  /**
   * Label of problem in contest
   * @minLength 1
   */
  label: string;
}

/** Submit result information */
export interface SubmitResultExtendedInfo {
  /**
   * Submit result id
   * @format int64
   */
  submitResultId: number;
  /**
   * Submit date
   * @format date-time
   */
  submitDate: string;
  /**
   * Language name
   * @minLength 1
   */
  language: string;
  status: SubmitStatus;
  /**
   * Total tests passed
   * @format int32
   */
  passedTests?: number | null;
  /**
   * Max execution time in milliseconds
   * @format int64
   */
  totalMilliseconds?: number | null;
  /**
   * Max memory consumed in bytes
   * @format int64
   */
  totalBytes?: number | null;
  /**
   * Problem id
   * @format int64
   */
  problemId: number;
  /**
   * Problem name
   * @minLength 1
   */
  problemName: string;
  /**
   * User id
   * @format int64
   */
  userId: number;
  /**
   * User name
   * @minLength 1
   */
  userName: string;
  /** Contest information */
  contestInfo?: SubmitResultContestInfo;
  /**
   * Source code of solution
   * @minLength 1
   */
  sourceCode: string;
  /** Compiler output */
  compilerOutput?: string | null;
  /** Text output of run.exe */
  runOutput?: string | null;
  /** User host of submit */
  userHost?: string | null;
}

/** Submit result information */
export interface SubmitResultInfo {
  /**
   * Submit result id
   * @format int64
   */
  submitResultId: number;
  /**
   * Submit date
   * @format date-time
   */
  submitDate: string;
  /**
   * Language name
   * @minLength 1
   */
  language: string;
  status: SubmitStatus;
  /**
   * Total tests passed
   * @format int32
   */
  passedTests?: number | null;
  /**
   * Max execution time in milliseconds
   * @format int64
   */
  totalMilliseconds?: number | null;
  /**
   * Max memory consumed in bytes
   * @format int64
   */
  totalBytes?: number | null;
  /**
   * Problem id
   * @format int64
   */
  problemId: number;
  /**
   * Problem name
   * @minLength 1
   */
  problemName: string;
  /**
   * User id
   * @format int64
   */
  userId: number;
  /**
   * User name
   * @minLength 1
   */
  userName: string;
  /** Contest information */
  contestInfo?: SubmitResultContestInfo;
}

/** Submit results list */
export interface SubmitResultsList {
  /** Submit results */
  items: SubmitResultInfo[];
  /**
   * Total count of submit results
   * @format int32
   */
  totalCount: number;
}

export enum SubmitStatus {
  Pending = "Pending",
  CompilationError = "CompilationError",
  RuntimeError = "RuntimeError",
  TimeLimitExceeded = "TimeLimitExceeded",
  MemoryLimitExceeded = "MemoryLimitExceeded",
  WrongAnswer = "WrongAnswer",
  Accepted = "Accepted",
  ServerError = "ServerError",
  TooEarly = "TooEarly",
  Unpolite = "Unpolite",
  TooManyLines = "TooManyLines",
}

/** User information */
export interface User {
  /**
   * User id
   * @format int64
   */
  id: number;
  /**
   * User login
   * @minLength 1
   */
  login: string;
  /**
   * User email
   * @minLength 1
   */
  email: string;
}

/** Users list */
export interface UsersList {
  /** Users */
  items: User[];
  /**
   * Total users count
   * @format int32
   */
  totalCount: number;
}

export type QueryParamsType = Record<string | number, any>;
export type ResponseFormat = keyof Omit<Body, "body" | "bodyUsed">;

export interface FullRequestParams extends Omit<RequestInit, "body"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseFormat;
  /** request body */
  body?: unknown;
  /** base url */
  baseUrl?: string;
  /** request cancellation token */
  cancelToken?: CancelToken;
}

export type RequestParams = Omit<FullRequestParams, "body" | "method" | "query" | "path">;

export interface ApiConfig<SecurityDataType = unknown> {
  baseUrl?: string;
  baseApiParams?: Omit<RequestParams, "baseUrl" | "cancelToken" | "signal">;
  securityWorker?: (securityData: SecurityDataType | null) => Promise<RequestParams | void> | RequestParams | void;
  customFetch?: typeof fetch;
}

export interface HttpResponse<D extends unknown, E extends unknown = unknown> extends Response {
  data: D;
  error: E;
}

type CancelToken = Symbol | string | number;

export enum ContentType {
  Json = "application/json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public baseUrl: string = "";
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private abortControllers = new Map<CancelToken, AbortController>();
  private customFetch = (...fetchParams: Parameters<typeof fetch>) => fetch(...fetchParams);

  private baseApiParams: RequestParams = {
    credentials: "same-origin",
    headers: {},
    redirect: "follow",
    referrerPolicy: "no-referrer",
  };

  constructor(apiConfig: ApiConfig<SecurityDataType> = {}) {
    Object.assign(this, apiConfig);
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected encodeQueryParam(key: string, value: any) {
    const encodedKey = encodeURIComponent(key);
    return `${encodedKey}=${encodeURIComponent(typeof value === "number" ? value : `${value}`)}`;
  }

  protected addQueryParam(query: QueryParamsType, key: string) {
    return this.encodeQueryParam(key, query[key]);
  }

  protected addArrayQueryParam(query: QueryParamsType, key: string) {
    const value = query[key];
    return value.map((v: any) => this.encodeQueryParam(key, v)).join("&");
  }

  protected toQueryString(rawQuery?: QueryParamsType): string {
    const query = rawQuery || {};
    const keys = Object.keys(query).filter((key) => "undefined" !== typeof query[key]);
    return keys
      .map((key) => (Array.isArray(query[key]) ? this.addArrayQueryParam(query, key) : this.addQueryParam(query, key)))
      .join("&");
  }

  protected addQueryParams(rawQuery?: QueryParamsType): string {
    const queryString = this.toQueryString(rawQuery);
    return queryString ? `?${queryString}` : "";
  }

  private contentFormatters: Record<ContentType, (input: any) => any> = {
    [ContentType.Json]: (input: any) =>
      input !== null && (typeof input === "object" || typeof input === "string") ? JSON.stringify(input) : input,
    [ContentType.Text]: (input: any) => (input !== null && typeof input !== "string" ? JSON.stringify(input) : input),
    [ContentType.FormData]: (input: any) =>
      Object.keys(input || {}).reduce((formData, key) => {
        const property = input[key];
        formData.append(
          key,
          property instanceof Blob
            ? property
            : typeof property === "object" && property !== null
            ? JSON.stringify(property)
            : `${property}`,
        );
        return formData;
      }, new FormData()),
    [ContentType.UrlEncoded]: (input: any) => this.toQueryString(input),
  };

  protected mergeRequestParams(params1: RequestParams, params2?: RequestParams): RequestParams {
    return {
      ...this.baseApiParams,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...(this.baseApiParams.headers || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected createAbortSignal = (cancelToken: CancelToken): AbortSignal | undefined => {
    if (this.abortControllers.has(cancelToken)) {
      const abortController = this.abortControllers.get(cancelToken);
      if (abortController) {
        return abortController.signal;
      }
      return void 0;
    }

    const abortController = new AbortController();
    this.abortControllers.set(cancelToken, abortController);
    return abortController.signal;
  };

  public abortRequest = (cancelToken: CancelToken) => {
    const abortController = this.abortControllers.get(cancelToken);

    if (abortController) {
      abortController.abort();
      this.abortControllers.delete(cancelToken);
    }
  };

  public request = async <T = any, E = any>({
    body,
    secure,
    path,
    type,
    query,
    format,
    baseUrl,
    cancelToken,
    ...params
  }: FullRequestParams): Promise<HttpResponse<T, E>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.baseApiParams.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const queryString = query && this.toQueryString(query);
    const payloadFormatter = this.contentFormatters[type || ContentType.Json];
    const responseFormat = format || requestParams.format;

    return this.customFetch(`${baseUrl || this.baseUrl || ""}${path}${queryString ? `?${queryString}` : ""}`, {
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type && type !== ContentType.FormData ? { "Content-Type": type } : {}),
      },
      signal: (cancelToken ? this.createAbortSignal(cancelToken) : requestParams.signal) || null,
      body: typeof body === "undefined" || body === null ? null : payloadFormatter(body),
    }).then(async (response) => {
      const r = response as HttpResponse<T, E>;
      r.data = null as unknown as T;
      r.error = null as unknown as E;

      const data = !responseFormat
        ? r
        : await response[responseFormat]()
            .then((data) => {
              if (r.ok) {
                r.data = data;
              } else {
                r.error = data;
              }
              return r;
            })
            .catch((e) => {
              r.error = e;
              return r;
            });

      if (cancelToken) {
        this.abortControllers.delete(cancelToken);
      }

      if (!response.ok) throw data;
      return data;
    });
  };
}

/**
 * @title Judge.NET
 * @version v1
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
  api = {
    /**
     * No description
     *
     * @tags Contests
     * @name ContestsList
     * @summary Search contests
     * @request GET:/api/contests
     * @secure
     */
    contestsList: (
      query?: {
        /**
         * Contests to skip
         * @format int32
         * @min 0
         * @max 2147483647
         */
        Skip?: number;
        /**
         * Max items in result
         * @format int32
         * @min 1
         * @max 100
         */
        Take?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<ContestsInfoList, any>({
        path: `/api/contests`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contests
     * @name ContestsUpdate
     * @summary Create or update contest information
     * @request PUT:/api/contests
     * @secure
     */
    contestsUpdate: (data: EditContest, params: RequestParams = {}) =>
      this.request<EditContest, any>({
        path: `/api/contests`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contests
     * @name ContestsDetail
     * @summary Get contest by id
     * @request GET:/api/contests/{contestId}
     * @secure
     */
    contestsDetail: (contestId: number, params: RequestParams = {}) =>
      this.request<Contest, any>({
        path: `/api/contests/${contestId}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contests
     * @name ContestsResultsDetail
     * @summary Get contest results
     * @request GET:/api/contests/{contestId}/results
     * @secure
     */
    contestsResultsDetail: (contestId: number, params: RequestParams = {}) =>
      this.request<ContestResult, any>({
        path: `/api/contests/${contestId}/results`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Login
     * @name LoginTokenCreate
     * @summary Create authorization token
     * @request POST:/api/login/token
     * @secure
     */
    loginTokenCreate: (data: Login, params: RequestParams = {}) =>
      this.request<LoginResult, any>({
        path: `/api/login/token`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Problems
     * @name ProblemsList
     * @summary Search problems
     * @request GET:/api/problems
     * @secure
     */
    problemsList: (
      query?: {
        /**
         * Problem name
         * @minLength 2
         */
        Name?: string;
        /**
         * Problems to skip
         * @format int32
         * @min 0
         * @max 2147483647
         */
        Skip?: number;
        /**
         * Problems to take
         * @format int32
         * @min 1
         * @max 100
         */
        Take?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<ProblemsList, any>({
        path: `/api/problems`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Problems
     * @name ProblemsUpdate
     * @summary Crate or edit new problem
     * @request PUT:/api/problems
     * @secure
     */
    problemsUpdate: (data: EditProblem, params: RequestParams = {}) =>
      this.request<EditProblem, any>({
        path: `/api/problems`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Problems
     * @name ProblemsDetail
     * @summary Get problem by id
     * @request GET:/api/problems/{id}
     * @secure
     */
    problemsDetail: (id: number, params: RequestParams = {}) =>
      this.request<Problem, any>({
        path: `/api/problems/${id}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Submits
     * @name SubmitsSubmitsList
     * @summary Search submits
     * @request GET:/api/submits/submits
     * @secure
     */
    submitsSubmitsList: (
      query?: {
        /**
         * Problem id
         * @format int64
         */
        ProblemId?: number;
        /**
         * Contest id
         * @format int32
         */
        ContestId?: number;
        /** Problem label */
        ProblemLabel?: string;
        /**
         * Submits to skip
         * @format int32
         * @min 0
         * @max 2147483647
         */
        Skip?: number;
        /**
         * Submits to take
         * @format int32
         * @min 1
         * @max 100
         */
        Take?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<SubmitResultsList, any>({
        path: `/api/submits/submits`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Submits
     * @name SubmitsSubmitsUpdate
     * @summary Submit solution
     * @request PUT:/api/submits/submits
     * @secure
     */
    submitsSubmitsUpdate: (
      data: {
        /**
         * File with solution
         * @format binary
         */
        File: File;
        /**
         * Language id
         * @format int32
         */
        LanguageId: number;
        /**
         * problem id
         * @format int64
         */
        ProblemId?: number;
        /**
         * Contest id
         * @format int32
         */
        ContestId?: number;
        /** Problem label in contest */
        ProblemLabel?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<void, ProblemDetails>({
        path: `/api/submits/submits`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.FormData,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Submits
     * @name SubmitsProblemsSubmitsDetail
     * @summary Get submit results for problem
     * @request GET:/api/submits/problems/{problemId}/submits
     * @secure
     */
    submitsProblemsSubmitsDetail: (
      problemId: number,
      query?: {
        /**
         * Submit results to skip
         * @format int32
         * @default 0
         */
        skip?: number;
        /**
         * Submit result to take
         * @format int32
         * @default 20
         */
        take?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<SubmitResultsList, any>({
        path: `/api/submits/problems/${problemId}/submits`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Submits
     * @name SubmitsSubmitsDetail
     * @summary Get submit result by id
     * @request GET:/api/submits/submits/{id}
     * @secure
     */
    submitsSubmitsDetail: (id: number, params: RequestParams = {}) =>
      this.request<SubmitResultExtendedInfo, any>({
        path: `/api/submits/submits/${id}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Submits
     * @name SubmitsSubmitsRejudgeCreate
     * @summary Rejudge submit
     * @request POST:/api/submits/submits/{id}/rejudge
     * @secure
     */
    submitsSubmitsRejudgeCreate: (id: number, params: RequestParams = {}) =>
      this.request<SubmitResultExtendedInfo, any>({
        path: `/api/submits/submits/${id}/rejudge`,
        method: "POST",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Users
     * @name UsersMeList
     * @summary Get current user information
     * @request GET:/api/users/me
     * @secure
     */
    usersMeList: (params: RequestParams = {}) =>
      this.request<User, any>({
        path: `/api/users/me`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Users
     * @name UsersList
     * @summary Search users
     * @request GET:/api/users
     * @secure
     */
    usersList: (
      query?: {
        /**
         * User name or email
         * @minLength 2
         */
        Name?: string;
        /**
         * Users to skip
         * @format int32
         * @min 0
         * @max 2147483647
         */
        Skip?: number;
        /**
         * Users to take
         * @format int32
         * @min 1
         * @max 100
         */
        Take?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<UsersList, any>({
        path: `/api/users`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Users
     * @name UsersUpdate
     * @summary Create new user
     * @request PUT:/api/users
     * @secure
     */
    usersUpdate: (data: CreateUser, params: RequestParams = {}) =>
      this.request<User, any>({
        path: `/api/users`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),
  };
}
