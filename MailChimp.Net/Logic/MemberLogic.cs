﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MailChimp.Net.Core;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;

namespace MailChimp.Net.Logic
{
    internal class MemberLogic : BaseLogic, IMemberLogic
    {
        public MemberLogic(string apiKey): base(apiKey){}
        
        public async Task<IEnumerable<Member>> GetAllAsync(string listId)
        {
            try
            {
                using (var client = CreateMailClient("lists/"))
                {
                    var response = await client.GetAsync($"{listId}/members");
                    response.EnsureSuccessStatusCode();

                    var listResponse = await response.Content.ReadAsAsync<MemberResponse>();
                    return listResponse.Members;
                }
            }
            catch (Exception ex)
            {
                
            }

            return null;
        }

        public async Task<Member> GetAsync(string listId, string emailAddress, bool isHashed = false)
        {
            try
            {
                using (var client = CreateMailClient("lists/"))
                {
                    var hashedEmailAddress = isHashed ? emailAddress : Hash(emailAddress);
                    var response = await client.GetAsync($"{listId}/members/{hashedEmailAddress}");
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsAsync<Member>();
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Member> AddOrUpdateAsync(string listId, Member member, string targetEmailAddress = null)
        {
            try
            {
                using (var client = CreateMailClient("lists/"))
                {
                    var hashedEmailAddress = string.IsNullOrWhiteSpace(targetEmailAddress)
                        ? Hash(member.EmailAddress)
                        : Hash(targetEmailAddress);
                    var response = await client.PutAsJsonAsync($"{listId}/members/{hashedEmailAddress}", member);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsAsync<Member>();
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

    }
}
