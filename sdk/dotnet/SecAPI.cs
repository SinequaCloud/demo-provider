// *** WARNING: this file was generated by pulumi. ***
// *** Do not edit by hand unless you're certain you know what you are doing! ***

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Pulumi.Serialization;

namespace Pulumi.Secapiprovider
{
    [SecapiproviderResourceType("secapiprovider:index:SecAPI")]
    public partial class SecAPI : global::Pulumi.CustomResource
    {
        [Output("groups")]
        public Output<string?> Groups { get; private set; } = null!;


        /// <summary>
        /// Create a SecAPI resource with the given unique name, arguments, and options.
        /// </summary>
        ///
        /// <param name="name">The unique name of the resource</param>
        /// <param name="args">The arguments used to populate this resource's properties</param>
        /// <param name="options">A bag of options that control this resource's behavior</param>
        public SecAPI(string name, SecAPIArgs args, CustomResourceOptions? options = null)
            : base("secapiprovider:index:SecAPI", name, args ?? new SecAPIArgs(), MakeResourceOptions(options, ""))
        {
        }

        private SecAPI(string name, Input<string> id, CustomResourceOptions? options = null)
            : base("secapiprovider:index:SecAPI", name, null, MakeResourceOptions(options, id))
        {
        }

        private static CustomResourceOptions MakeResourceOptions(CustomResourceOptions? options, Input<string>? id)
        {
            var defaultOptions = new CustomResourceOptions
            {
                Version = Utilities.Version,
            };
            var merged = CustomResourceOptions.Merge(defaultOptions, options);
            // Override the ID if one was specified for consistency with other language SDKs.
            merged.Id = id ?? merged.Id;
            return merged;
        }
        /// <summary>
        /// Get an existing SecAPI resource's state with the given name, ID, and optional extra
        /// properties used to qualify the lookup.
        /// </summary>
        ///
        /// <param name="name">The unique name of the resulting resource.</param>
        /// <param name="id">The unique provider ID of the resource to lookup.</param>
        /// <param name="options">A bag of options that control this resource's behavior</param>
        public static SecAPI Get(string name, Input<string> id, CustomResourceOptions? options = null)
        {
            return new SecAPI(name, id, options);
        }
    }

    public sealed class SecAPIArgs : global::Pulumi.ResourceArgs
    {
        [Input("env", required: true)]
        public Input<string> Env { get; set; } = null!;

        [Input("org", required: true)]
        public Input<string> Org { get; set; } = null!;

        public SecAPIArgs()
        {
        }
        public static new SecAPIArgs Empty => new SecAPIArgs();
    }
}
