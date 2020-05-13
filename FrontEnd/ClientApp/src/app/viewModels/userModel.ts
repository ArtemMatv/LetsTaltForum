interface IUser {
    id: number,
    userName: string,
    password: string,
    email: string,
    age: number,
    gender: string,
    avatarPath: string,
    comments: IComment[],
    posts: IPost[],
    userRole: IRole,
    silencedTo: string,
    bannedTo: string,
    token?: string
}

interface IRole {
    id: number,
    name: string
}
