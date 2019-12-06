using System.Collections.Generic;
using System.Threading.Tasks;
using UW.Web.Services.GWSClient.Models;
using System.Threading;

namespace UW.Web.Services.GWSClient
{
    public interface IGWSClient
    {
        /// <summary>
        /// Searches GroupsWS for groups starting with the provided stemId and depth
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list of groups found matching the stemId</returns>
        Task<SearchResult> Search(
            SearchRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Replace group members with member list (one memberType at at a time)
        /// </summary>
        /// <param name="req">ReplaceMembersRequest object.</param>
        /// <returns>A flag representing if members were successfully replaced</returns>
        Task<AddMembersResult> ReplaceMembers(
            ReplaceMembersRequest req,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add multiple members to a group
        /// </summary>
        /// <param name="req">AddMembersRequest object.</param>
        /// <returns>A flag representing if members were successfully added</returns>
        Task<AddMembersResult> AddMembers(AddMembersRequest req,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add one member to a group
        /// </summary>
        /// <param name="req">AddMemberRequest object.</param>
        /// <returns>A flag representing if the action was completed successfully</returns>
        Task<AddMembersResult> AddMember(AddMemberRequest req,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Lookup groups in GroupsWS for additional information.
        /// </summary>
        /// <param name="group">The group to lookup</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of groups found with additional information</returns>
        Task<GetInfoResult> GetInfo(string group,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Lookup groups in GroupsWS for additional information.
        /// </summary>
        /// <param name="req">GetInfoRequest object</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of groups found with additional information</returns>
        Task<IEnumerable<GetInfoResult>> GetInfo(GetInfosRequest req,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get history data for a specific group
        /// </summary>
        /// <param name="group">The group for which to get history</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The group history</returns>
        Task<GetHistoryResult> GetHistory(UWGroup group,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete groups in GroupsWS
        /// </summary>
        /// <param name="group">Group to delete.</param>
        /// <param name="synchronized">Wait until the group has been fully deleted before returning? (default: true)</param>
        /// <returns>A list of groups deleted in GroupsWS</returns>
        Task<DeleteGroupResult> Delete(string group,
            bool synchronized = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete groups in GroupsWS
        /// </summary>
        /// <param name="req">DeleteGroupsRequest object.</param>
        /// <returns>A list of groups deleted in GroupsWS</returns>
        Task<IEnumerable<DeleteGroupResult>> Delete(DeleteGroupsRequest req,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create a new group
        /// </summary>
        /// <returns></returns>
        Task<CreateGroupResult> Create(
            CreateGroupRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove members from a group
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Removal results</returns>
        Task<RemoveMembersResult> RemoveMembers(RemoveMembersRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get group direct members
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of group members</returns>
        Task<GetMembersResult> GetMembers(GetMembersRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get group effective members.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of effective group members.</returns>
        Task<GetEffectiveMembersResult> GetEffectiveMembers(GetMembersRequest request,
            CancellationToken cancellationToken = default);
    }
}
