struct AbstractModelWithId: Codable {
    let Id: String

    let InsertUserName: String
    let InsertDate: String?

    let UpdateUserName: String
    let UpdateDate: String?

    let RemoveIs: Bool
    let RemoveUserName: String
    let RemoveDate: String?
}
